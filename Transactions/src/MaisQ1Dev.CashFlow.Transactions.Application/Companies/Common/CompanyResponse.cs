using MaisQ1Dev.CashFlow.Transactions.Domain.Companies;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Companies.Common;

public sealed record CompanyResponse(Guid Id, string Name, string Email)
{
    public static CompanyResponse FromCompany(Company company)
        => new(company.Id, company.Name, company.Email);
}
