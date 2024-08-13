using MaisQ1Dev.CashFlow.Transactions.Application.Transactions.Common;
using MaisQ1Dev.Libs.Domain;
using MediatR;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Transactions.GetTransactionById;

public sealed record GetTransactionByIdQuery(Guid Id) : IRequest<Result<TransactionResponse>>;
