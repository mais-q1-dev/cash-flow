using MaisQ1Dev.CashFlow.Transactions.Domain.Transactions;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Transactions.Common;

public sealed record TransactionResponse(
    Guid Id,
    Guid CompanyId,
    ETransactionType Type,
    DateTime Date,
    decimal Amount,
    string? Description)
{
    public static TransactionResponse FromTransaction(Transaction transaction)
        => new(
            transaction.Id,
            transaction.CompanyId,
            transaction.Type,
            transaction.Date,
            transaction.Amount,
            transaction.Description);
};