using MaisQ1Dev.CashFlow.Reports.Domain.Transactions;

namespace MaisQ1Dev.CashFlow.Reports.Application.Transactions.Common;

public sealed record TransactionResponse(
    Guid Id,
    Guid CompanyId,
    DateTime Date,
    decimal Amount,
    string? Description)
{
    public static TransactionResponse FromTransaction(Transaction transaction)
        => new(
            transaction.Id,
            transaction.CompanyId,
            transaction.Date,
            transaction.Amount,
            transaction.Description);
};