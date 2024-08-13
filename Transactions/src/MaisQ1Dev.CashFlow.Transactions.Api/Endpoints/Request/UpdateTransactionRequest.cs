using MaisQ1Dev.CashFlow.Transactions.Application.Transactions.UpdateTransaction;
using MaisQ1Dev.CashFlow.Transactions.Domain.Transactions;

namespace MaisQ1Dev.CashFlow.Transactions.Api.Endpoints.Request;

public sealed record UpdateTransactionRequest(
    Guid CompanyId,
    ETransactionType Type,
    DateTime Date,
    decimal Amount,
    string? Description)
{
    public UpdateTransactionCommand ToCommand(Guid id)
        => new(id, CompanyId, Type, Date, Amount, Description);
}