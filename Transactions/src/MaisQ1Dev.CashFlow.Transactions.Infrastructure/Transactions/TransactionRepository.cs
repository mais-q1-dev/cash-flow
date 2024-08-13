using MaisQ1Dev.CashFlow.Transactions.Domain.Transactions;
using MaisQ1Dev.CashFlow.Transactions.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MaisQ1Dev.CashFlow.Transactions.Infrastructure.Transactions;

public class TransactionRepository : ITransactionRepository
{
    private readonly CashFlowTransactionDbContext _context;

    public TransactionRepository(CashFlowTransactionDbContext context)
        => _context = context;

    public async Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken)
        => await _context.Transactions.AddAsync(transaction, cancellationToken);

    public void Update(Transaction transaction)
        => _context.Transactions.Update(transaction);
}
