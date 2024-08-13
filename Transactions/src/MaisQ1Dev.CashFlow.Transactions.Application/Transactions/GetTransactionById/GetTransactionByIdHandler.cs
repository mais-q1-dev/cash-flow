using MaisQ1Dev.CashFlow.Transactions.Application.Abstractions.Data;
using MaisQ1Dev.CashFlow.Transactions.Application.Transactions.Common;
using MaisQ1Dev.CashFlow.Transactions.Domain.Transactions;
using MaisQ1Dev.Libs.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Transactions.GetTransactionById;

public sealed class GetTransactionByIdHandler : IRequestHandler<GetTransactionByIdQuery, Result<TransactionResponse>>
{
    private readonly ICashFlowTransactionDbContext _context;

    public GetTransactionByIdHandler(ICashFlowTransactionDbContext context)
        => _context = context;

    public async Task<Result<TransactionResponse>> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var transactionResponse = await _context
            .Transactions
            .AsNoTracking()
            .Where(c => c.Id == request.Id)
            .Select(transaction => TransactionResponse.FromTransaction(transaction))
            .FirstOrDefaultAsync(cancellationToken);

        if (transactionResponse is null)
            return Result.NotFound<TransactionResponse>(TransactionError.NotFound);

        return Result.Ok(transactionResponse);
    }
}