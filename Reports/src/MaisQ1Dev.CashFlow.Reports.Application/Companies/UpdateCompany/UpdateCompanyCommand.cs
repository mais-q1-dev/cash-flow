using MaisQ1Dev.Libs.Domain;
using MediatR;

namespace MaisQ1Dev.CashFlow.Reports.Application.Companies.UpdateCompany;

public sealed record UpdateCompanyCommand(Guid CompanyId, string Name, string Email) : IRequest<Result>;
