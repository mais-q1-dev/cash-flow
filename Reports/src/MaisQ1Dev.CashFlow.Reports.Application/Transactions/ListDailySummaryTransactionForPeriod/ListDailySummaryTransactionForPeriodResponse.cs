namespace MaisQ1Dev.CashFlow.Reports.Application.Transactions.ListDailySummaryTransactionForPeriod;

public sealed record ListDailySummaryTransactionForPeriodResponse(
    DateTime Date,
    decimal Income,
    decimal Expense,
    decimal Total);
