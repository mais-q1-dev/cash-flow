using MaisQ1Dev.Libs.Domain;
using MediatR;

namespace MaisQ1Dev.CashFlow.Reports.Application.Transactions.ListDailySummaryTransactionForPeriod;

public sealed record ListDailySummaryTransactionForPeriodQuery(
    Guid CompanyId,
    DateTime StartDate,
    DateTime EndDate) : IRequest<Result<IEnumerable<ListDailySummaryTransactionForPeriodResponse>>>;
