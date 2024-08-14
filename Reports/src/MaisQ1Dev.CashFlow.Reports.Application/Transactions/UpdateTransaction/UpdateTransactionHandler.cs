using MaisQ1Dev.CashFlow.Reports.Domain.Companies;
using MaisQ1Dev.CashFlow.Reports.Domain.Transactions;
using MaisQ1Dev.Libs.Domain;
using MaisQ1Dev.Libs.Domain.Database;
using MaisQ1Dev.Libs.Domain.Logging;
using MaisQ1Dev.Libs.IntegrationEvents.EventBus;
using MaisQ1Dev.Libs.IntegrationEvents.Transaction;
using MediatR;

namespace MaisQ1Dev.CashFlow.Reports.Application.Transactions.UpdateTransaction;

public sealed class UpdateTransactionHandler : IRequestHandler<UpdateTransactionCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICompanyRepository _companyRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IEventBus _eventBus;
    private readonly ILoggerMQ1Dev<UpdateTransactionHandler> _logger;

    public UpdateTransactionHandler(
        IUnitOfWork unitOfWork,
        ICompanyRepository companyRepository,
        ITransactionRepository transactionRepository,
        IEventBus eventBus,
        ILoggerMQ1Dev<UpdateTransactionHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _companyRepository = companyRepository;
        _transactionRepository = transactionRepository;
        _eventBus = eventBus;
        _logger = logger;
    }


    public async Task<Result> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId, default);
        if (transaction is null)
        {
            _logger.LogError("Transaction {TransactionId} not found", request.TransactionId);
            return Result.NotFound(TransactionError.NotFound);
        }

        if (transaction.CompanyId != request.CompanyId)
        {
            _logger.LogError(
                "Transaction {TransactionId} does not belong to company {CompanyId}", 
                request.TransactionId, 
                request.CompanyId);

            return Result.NotFound(TransactionError.NotFound);
        }

        var company = await _companyRepository.GetByIdAsync(transaction.CompanyId, default);
        if (company is null)
        {
            _logger.LogError("Company {CompanyId} not found", transaction.CompanyId);
            return Result.NotFound(CompanyError.NotFound);
        }

        var oldAmount = transaction.Amount * -1;
        transaction.Update(request.Date, request.Amount, request.Description);

        company.UpdateBalance(oldAmount);
        company.UpdateBalance(request.Amount);
        _logger.LogInformation("Company {CompanyId} balance updated", company.Id);

        _transactionRepository.Update(transaction);
        _companyRepository.Update(company);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Transaction {TransactionId} updated", transaction.Id);

        _logger.LogInformation("Publishing the event to integrate the transaction {TransactionId}");
        var @event = new TransactionSyncIntegrationEvent(transaction.Id);
        await _eventBus.PublishAsync(@event);
        _logger.LogInformation("Event published successfully");

        return Result.NoContent();
    }
}
