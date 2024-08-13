using MaisQ1Dev.Libs.Domain;
using MediatR;

namespace MaisQ1Dev.CashFlow.Reports.Application.Companies.CreateCompany;

public sealed record CreateCompanyCommand(Guid CompanyId, string Name, string Email) : IRequest<Result<Guid>>;