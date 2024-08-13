using MaisQ1Dev.CashFlow.Reports.Application.Abstractions.Data;
using MaisQ1Dev.Libs.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaisQ1Dev.CashFlow.Reports.Application.Transactions.ListDailySummaryTransactionForPeriod;

public sealed class ListDailySummaryTransactionForPeriodHandler
    : IRequestHandler<ListDailySummaryTransactionForPeriodQuery, Result<IEnumerable<ListDailySummaryTransactionForPeriodResponse>>>
{
    private readonly ICashFlowReportDbContext _context;

    public ListDailySummaryTransactionForPeriodHandler(ICashFlowReportDbContext context)
        => _context = context;

    public async Task<Result<IEnumerable<ListDailySummaryTransactionForPeriodResponse>>> Handle(
        ListDailySummaryTransactionForPeriodQuery request,
        CancellationToken cancellationToken)
    {
        var allDatesInPeriod = Enumerable.Range(0, 1 + request.EndDate.Subtract(request.StartDate).Days)
            .Select(offset => request.StartDate.AddDays(offset).Date)
            .ToList();

        var transactionSummaries = await _context.Transactions
            .Where(t =>
                t.CompanyId == request.CompanyId &&
                t.Date >= request.StartDate.ToUniversalTime() &&
                t.Date <= request.EndDate.ToUniversalTime())
            .OrderBy(t => t.Date.Date)
            .GroupBy(t => t.Date.Date)
            .Select(g => new ListDailySummaryTransactionForPeriodResponse(
                g.Key,
                g.Where(t => t.Amount > 0).Sum(t => t.Amount),
                g.Where(t => t.Amount < 0).Sum(t => t.Amount),
                g.Sum(t => t.Amount)
            ))
            .ToListAsync(cancellationToken)
            ?? Enumerable.Empty<ListDailySummaryTransactionForPeriodResponse>();

        var summaries = allDatesInPeriod
            .GroupJoin(
                transactionSummaries,
                date => date,
                summary => summary.Date,
                (date, transactionSummary)
                    => transactionSummary
                        .DefaultIfEmpty(new ListDailySummaryTransactionForPeriodResponse(date, 0, 0, 0))
                        .First()
            )
            .OrderBy(r => r.Date)
            .ToList();

        return Result.Ok(summaries.AsEnumerable());
    }
}
