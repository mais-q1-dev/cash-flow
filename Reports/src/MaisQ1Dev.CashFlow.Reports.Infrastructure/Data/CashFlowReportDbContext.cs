using MaisQ1Dev.CashFlow.Reports.Application.Abstractions.Data;
using MaisQ1Dev.CashFlow.Reports.Domain.Companies;
using MaisQ1Dev.CashFlow.Reports.Domain.Transactions;
using MaisQ1Dev.Libs.Domain.Database;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace MaisQ1Dev.CashFlow.Reports.Infrastructure.Data;

public class CashFlowReportDbContext : DbContext, ICashFlowReportDbContext, IUnitOfWork
{
    public CashFlowReportDbContext(DbContextOptions<CashFlowReportDbContext> options)
        : base(options)
    { }

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CashFlowReportDbContext).Assembly);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();

        base.OnModelCreating(modelBuilder);
    }
}