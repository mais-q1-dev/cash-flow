namespace MaisQ1Dev.CashFlow.Reports.Application.Companies.GetCompanyById;

public sealed record GetCompanyByIdResponse(Guid Id, string Name, string Email, decimal Balance);

