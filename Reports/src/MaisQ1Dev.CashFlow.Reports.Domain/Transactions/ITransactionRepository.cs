namespace MaisQ1Dev.CashFlow.Reports.Domain.Transactions;

public interface ITransactionRepository
{
    Task<bool> Exists(Guid id, CancellationToken cancellationToken);
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken);
    void Update(Transaction transaction);
}
