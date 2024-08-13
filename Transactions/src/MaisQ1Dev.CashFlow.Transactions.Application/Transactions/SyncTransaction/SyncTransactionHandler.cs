using MaisQ1Dev.CashFlow.Transactions.Domain.Transactions;
using MaisQ1Dev.Libs.Domain;
using MaisQ1Dev.Libs.Domain.Database;
using MediatR;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Transactions.SyncTransaction;

public class SyncTransactionHandler : IRequestHandler<SyncTransactionCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionRepository _transactionRepository;

    public SyncTransactionHandler(
        IUnitOfWork unitOfWork,
        ITransactionRepository transactionRepository)
    {
        _unitOfWork = unitOfWork;
        _transactionRepository = transactionRepository;
    }

    public async Task<Result> Handle(SyncTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId, default);
        if (transaction is null)
        {
            //_logger.LogError("Transaction not found for id [{TransactionId}]", context.Message.TransactionId);
            return Result.NotFound(TransactionError.NotFound);
        }

        transaction.Sync();

        _transactionRepository.Update(transaction);
        await _unitOfWork.SaveChangesAsync();

        //_logger.LogInformation("Transaction [{TransactionId}] synchronized", context.Message.TransactionId);
        return Result.NoContent();
    }
}
