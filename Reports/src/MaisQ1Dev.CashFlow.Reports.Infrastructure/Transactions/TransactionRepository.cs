using MaisQ1Dev.CashFlow.Reports.Domain.Transactions;
using MaisQ1Dev.CashFlow.Reports.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MaisQ1Dev.CashFlow.Reports.Infrastructure.Transactions;

public class TransactionRepository : ITransactionRepository
{
    private readonly CashFlowReportDbContext _context;

    public TransactionRepository(CashFlowReportDbContext context)
        => _context = context;

    public async Task<bool> Exists(Guid id, CancellationToken cancellationToken)
        => await _context.Transactions.AnyAsync(t => t.Id == id, cancellationToken);

    public async Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken)
        => await _context.Transactions.AddAsync(transaction, cancellationToken);

    public void Update(Transaction transaction)
        => _context.Transactions.Update(transaction);
}
