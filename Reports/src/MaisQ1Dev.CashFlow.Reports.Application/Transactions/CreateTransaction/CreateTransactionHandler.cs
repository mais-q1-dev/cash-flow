using MaisQ1Dev.CashFlow.Reports.Domain.Companies;
using MaisQ1Dev.CashFlow.Reports.Domain.Transactions;
using MaisQ1Dev.Libs.Domain;
using MaisQ1Dev.Libs.Domain.Database;
using MaisQ1Dev.Libs.Domain.Logging;
using MaisQ1Dev.Libs.IntegrationEvents.EventBus;
using MaisQ1Dev.Libs.IntegrationEvents.Transaction;
using MediatR;

namespace MaisQ1Dev.CashFlow.Reports.Application.Transactions.CreateTransaction;

public sealed class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICompanyRepository _companyRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IEventBus _eventBus;
    private readonly ILoggerMQ1Dev<CreateTransactionHandler> _logger;

    public CreateTransactionHandler(
        IUnitOfWork unitOfWork,
        ICompanyRepository companyRepository,
        ITransactionRepository transactionRepository,
        IEventBus eventBus,
        ILoggerMQ1Dev<CreateTransactionHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _companyRepository = companyRepository;
        _transactionRepository = transactionRepository;
        _eventBus = eventBus;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transactionExists = await _transactionRepository.Exists(request.TransactionId, default);
        if (transactionExists)
        {
            _logger.LogError("Transaction {TransactionId} already exists", request.TransactionId);
            return Result.UnprocessableEntity<Guid>(TransactionError.AlreadyExists);
        }

        var company = await _companyRepository.GetByIdAsync(request.CompanyId, default);
        if (company is null)
        {
            _logger.LogError("Company {CompanyId} not found", request.CompanyId);
            return Result.NotFound<Guid>(CompanyError.NotFound);
        }

        var transaction = Transaction.Create(
            request.TransactionId,
            company.Id,
            request.Date,
            request.Amount,
            request.Description);

        company.UpdateBalance(request.Amount);
        _logger.LogInformation("Company {CompanyId} balance updated", company.Id);

        await _transactionRepository.AddAsync(transaction, default);
        _companyRepository.Update(company);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Transaction {TransactionId} created", transaction.Id);

        _logger.LogInformation("Publishing the event to integrate the transaction {TransactionId}", transaction.Id);
        var @event = new TransactionSyncIntegrationEvent(transaction.Id);
        await _eventBus.PublishAsync(@event);
        _logger.LogInformation("Event published successfully");

        return Result.Created(transaction.Id);
    }
}
