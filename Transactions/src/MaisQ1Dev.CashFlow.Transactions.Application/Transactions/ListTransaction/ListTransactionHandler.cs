using MaisQ1Dev.CashFlow.Transactions.Application.Abstractions.Data;
using MaisQ1Dev.CashFlow.Transactions.Application.Transactions.Common;
using MaisQ1Dev.Libs.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Transactions.ListTransaction;

public sealed class ListTransactionHandler : IRequestHandler<ListTransactionQuery, PagedResult<IEnumerable<TransactionResponse>>>
{
    private readonly ICashFlowTransactionDbContext _context;

    public ListTransactionHandler(ICashFlowTransactionDbContext context)
        => _context = context;

    public async Task<PagedResult<IEnumerable<TransactionResponse>>> Handle(ListTransactionQuery request, CancellationToken cancellationToken)
    {
        var query = _context
            .Transactions
            .AsNoTracking()
            .Where(t =>
                t.CompanyId == request.CompanyId &&
                t.Date >= request.StartDate.ToUniversalTime() &&
                t.Date <= request.EndDate.ToUniversalTime())
            .OrderBy(t => t.Date)
            .Select(transaction => TransactionResponse.FromTransaction(transaction));

        var transactions = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var count = await query.CountAsync(cancellationToken);

        return new PagedResult<IEnumerable<TransactionResponse>>(
            request.PageNumber,
            request.PageSize,
            count,
            transactions);
    }
}
