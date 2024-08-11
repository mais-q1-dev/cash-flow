using MaisQ1Dev.CashFlow.Transactions.Application.Abstractions.Data;
using MaisQ1Dev.CashFlow.Transactions.Application.Companies.Common;
using MaisQ1Dev.Libs.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Companies.ListCompany;

public sealed class ListCompanyHandler : IRequestHandler<ListCompanyQuery, Result<IEnumerable<CompanyResponse>>>
{
    private readonly ICashFlowTransactionDbContext _context;

    public ListCompanyHandler(ICashFlowTransactionDbContext context)
        => _context = context;

    public async Task<Result<IEnumerable<CompanyResponse>>> Handle(ListCompanyQuery request, CancellationToken cancellationToken)
    {
        var companyResponseList = await _context
            .Companies
            .AsNoTracking()
            .OrderBy(comparer => comparer.Name)
            .Select(company => CompanyResponse.FromCompany(company))
            .ToListAsync(cancellationToken)
            ?? Enumerable.Empty<CompanyResponse>();
        
        return Result.Ok(companyResponseList);
    }
}
