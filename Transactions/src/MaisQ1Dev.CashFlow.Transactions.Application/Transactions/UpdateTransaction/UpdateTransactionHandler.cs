using MaisQ1Dev.CashFlow.Transactions.Domain.Companies;
using MaisQ1Dev.CashFlow.Transactions.Domain.Transactions;
using MaisQ1Dev.Libs.Domain;
using MaisQ1Dev.Libs.Domain.Database;
using MaisQ1Dev.Libs.Domain.Logging;
using MediatR;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Transactions.UpdateTransaction;

public sealed class UpdateTransactionHandler : IRequestHandler<UpdateTransactionCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICompanyRepository _companyRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILoggerMQ1Dev<UpdateTransactionHandler> _logger;

    public UpdateTransactionHandler(
        IUnitOfWork unitOfWork,
        ICompanyRepository companyRepository,
        ITransactionRepository transactionRepository,
        ILoggerMQ1Dev<UpdateTransactionHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _companyRepository = companyRepository;
        _transactionRepository = transactionRepository;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.Id, cancellationToken);
        if (transaction is null)
        {
            _logger.LogError("Transaction {TransactionId} not found for update", request.Id);
            return Result.NotFound(TransactionError.NotFound);
        }

        if (transaction.CompanyId != request.CompanyId)
        {
            _logger.LogError(
                "Transaction {TransactionId} does not belong to company {CompanyId}",
                transaction.Id,
                request.CompanyId);

            return Result.NotFound(CompanyError.NotFound);
        }

        transaction.Update(
            request.Type,
            request.Date,
            request.Amount,
            request.Description);

        _transactionRepository.Update(transaction);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Transaction {TransactionId} updated for company {CompanyId}",
            transaction.Id,
            transaction.CompanyId);

        return Result.NoContent();
    }
}
