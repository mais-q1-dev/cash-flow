using MaisQ1Dev.CashFlow.Transactions.Application.Companies.UpdateCompany;

namespace MaisQ1Dev.CashFlow.Transactions.Api.Endpoints.Request;

public sealed record UpdateCompanyRequest(
    string Name,
    string Email)
{
    public UpdateCompanyCommand ToCommand(Guid id) => new(id, Name, Email);
}