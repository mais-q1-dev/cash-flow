using MaisQ1Dev.CashFlow.Transactions.Application.Abstractions.Data;
using MaisQ1Dev.CashFlow.Transactions.Application.Companies.Common;
using MaisQ1Dev.CashFlow.Transactions.Domain.Companies;
using MaisQ1Dev.Libs.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Companies.GetCompanyById;

public sealed class GetCompanyByIdHandler : IRequestHandler<GetCompanyByIdQuery, Result<CompanyResponse>>
{
    private readonly ICashFlowTransactionDbContext _context;

    public GetCompanyByIdHandler(ICashFlowTransactionDbContext context)
        => _context = context;

    public async Task<Result<CompanyResponse>> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        var companyResponse = await _context
            .Companies
            .AsNoTracking()
            .Where(c => c.Id == request.Id)
            .Select(company => CompanyResponse.FromCompany(company))
            .FirstOrDefaultAsync(cancellationToken);

        if (companyResponse is null)
            return Result.NotFound<CompanyResponse>(CompanyError.NotFound);

        return Result.Ok(companyResponse);
    }
}
