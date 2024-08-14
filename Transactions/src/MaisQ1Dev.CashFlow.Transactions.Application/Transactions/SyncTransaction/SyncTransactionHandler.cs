using MaisQ1Dev.CashFlow.Transactions.Domain.Transactions;
using MaisQ1Dev.Libs.Domain;
using MaisQ1Dev.Libs.Domain.Database;
using MaisQ1Dev.Libs.Domain.Logging;
using MediatR;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Transactions.SyncTransaction;

public class SyncTransactionHandler : IRequestHandler<SyncTransactionCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILoggerMQ1Dev<SyncTransactionHandler> _logger;

    public SyncTransactionHandler(
        IUnitOfWork unitOfWork,
        ITransactionRepository transactionRepository,
        ILoggerMQ1Dev<SyncTransactionHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _transactionRepository = transactionRepository;
        _logger = logger;
    }

    public async Task<Result> Handle(SyncTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId, default);
        if (transaction is null)
        {
            _logger.LogError(
                "Transaction {TransactionId} not found for synchronization",
                request.TransactionId);
            
            return Result.NotFound(TransactionError.NotFound);
        }

        transaction.Sync();

        _transactionRepository.Update(transaction);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Transaction {TransactionId} synchronized", request.TransactionId);
        return Result.NoContent();
    }
}
