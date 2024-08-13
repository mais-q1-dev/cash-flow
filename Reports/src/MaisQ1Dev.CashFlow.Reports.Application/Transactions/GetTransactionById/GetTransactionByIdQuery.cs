using MaisQ1Dev.CashFlow.Reports.Application.Transactions.Common;
using MaisQ1Dev.Libs.Domain;
using MediatR;

namespace MaisQ1Dev.CashFlow.Reports.Application.Transactions.GetTransactionById;

public sealed record GetTransactionByIdQuery(Guid Id) : IRequest<Result<TransactionResponse>>;
