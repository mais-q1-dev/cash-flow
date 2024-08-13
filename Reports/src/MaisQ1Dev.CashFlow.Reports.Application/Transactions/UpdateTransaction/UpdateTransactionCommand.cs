using MaisQ1Dev.Libs.Domain;
using MediatR;

namespace MaisQ1Dev.CashFlow.Reports.Application.Transactions.UpdateTransaction;

public sealed record UpdateTransactionCommand(
    Guid TransactionId,
    Guid CompanyId,
    DateTime Date,
    decimal Amount,
    string? Description) : IRequest<Result>;