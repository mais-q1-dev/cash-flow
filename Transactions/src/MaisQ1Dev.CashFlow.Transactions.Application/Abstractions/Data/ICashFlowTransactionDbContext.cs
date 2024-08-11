using MaisQ1Dev.CashFlow.Transactions.Domain.Companies;
using Microsoft.EntityFrameworkCore;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Abstractions.Data;

public interface ICashFlowTransactionDbContext
{
    DbSet<Company> Companies { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}