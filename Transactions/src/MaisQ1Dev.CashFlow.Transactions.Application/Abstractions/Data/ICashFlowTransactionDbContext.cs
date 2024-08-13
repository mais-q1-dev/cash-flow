using MaisQ1Dev.CashFlow.Transactions.Domain.Companies;
using MaisQ1Dev.CashFlow.Transactions.Domain.Transactions;
using Microsoft.EntityFrameworkCore;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Abstractions.Data;

public interface ICashFlowTransactionDbContext
{
    DbSet<Company> Companies { get; }
    DbSet<Transaction> Transactions { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}