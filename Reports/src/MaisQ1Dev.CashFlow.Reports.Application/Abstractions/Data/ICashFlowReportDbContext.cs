using MaisQ1Dev.CashFlow.Reports.Domain.Companies;
using Microsoft.EntityFrameworkCore;

namespace MaisQ1Dev.CashFlow.Reports.Application.Abstractions.Data;

public interface ICashFlowReportDbContext
{
    DbSet<Company> Companies { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
