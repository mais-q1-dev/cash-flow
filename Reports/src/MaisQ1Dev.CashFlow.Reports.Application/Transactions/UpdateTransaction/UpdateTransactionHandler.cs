using MaisQ1Dev.CashFlow.Reports.Domain.Companies;
using MaisQ1Dev.CashFlow.Reports.Domain.Transactions;
using MaisQ1Dev.Libs.Domain;
using MaisQ1Dev.Libs.Domain.Database;
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

    public UpdateTransactionHandler(
        IUnitOfWork unitOfWork,
        ICompanyRepository companyRepository,
        ITransactionRepository transactionRepository,
        IEventBus eventBus)
    {
        _unitOfWork = unitOfWork;
        _companyRepository = companyRepository;
        _transactionRepository = transactionRepository;
        _eventBus = eventBus;
    }


    public async Task<Result> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId, default);
        if (transaction is null)
            return Result.NotFound(TransactionError.NotFound);

        if (transaction.CompanyId != request.CompanyId)
            return Result.NotFound(TransactionError.NotFound);

        var company = await _companyRepository.GetByIdAsync(transaction.CompanyId, default);
        if (company is null)
            return Result.NotFound(CompanyError.NotFound);

        var oldAmount = transaction.Amount * -1;
        transaction.Update(request.Date, request.Amount, request.Description);

        company.UpdateBalance(oldAmount);
        company.UpdateBalance(request.Amount);

        _transactionRepository.Update(transaction);
        _companyRepository.Update(company);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var @event = new TransactionSyncIntegrationEvent(transaction.Id);
        await _eventBus.PublishAsync(@event);

        return Result.NoContent();
    }
}
