using MaisQ1Dev.Libs.Domain;
using MediatR;

namespace MaisQ1Dev.CashFlow.Reports.Application.Companies.GetCompanyById;

public sealed record GetCompanyByIdQuery(Guid Id) : IRequest<Result<GetCompanyByIdResponse>>;
