using MaisQ1Dev.CashFlow.Transactions.Application.Companies.Common;
using MaisQ1Dev.Libs.Domain;
using MediatR;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Companies.ListCompany;

public sealed record ListCompanyQuery: IRequest<Result<IEnumerable<CompanyResponse>>>;