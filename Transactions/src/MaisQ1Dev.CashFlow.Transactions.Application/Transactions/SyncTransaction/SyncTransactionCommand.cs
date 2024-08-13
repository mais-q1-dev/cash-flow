using MaisQ1Dev.Libs.Domain;
using MediatR;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Transactions.SyncTransaction;

public sealed record SyncTransactionCommand(Guid TransactionId) : IRequest<Result>;