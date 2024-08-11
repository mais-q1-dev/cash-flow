using MaisQ1Dev.CashFlow.Reports.Application.Abstractions.Data;
using MaisQ1Dev.CashFlow.Reports.Domain.Companies;
using MaisQ1Dev.Libs.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaisQ1Dev.CashFlow.Reports.Application.Companies.GetCompanyById;

public sealed class GetCompanyByIdHandler : IRequestHandler<GetCompanyByIdQuery, Result<GetCompanyByIdResponse>>
{
    private readonly ICashFlowReportDbContext _context;

    public GetCompanyByIdHandler(ICashFlowReportDbContext context)
        => _context = context;

    public async Task<Result<GetCompanyByIdResponse>> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        var companyResponse = await _context
            .Companies
            .AsNoTracking()
            .Where(c => c.Id == request.Id)
            .Select(company => new GetCompanyByIdResponse(
                company.Id,
                company.Name,
                company.Email.Address,
                company.Balance))
            .FirstOrDefaultAsync(cancellationToken);

        if (companyResponse is null)
            return Result.NotFound<GetCompanyByIdResponse>(CompanyError.NotFound);

        return Result.Ok(companyResponse);
    }
}
