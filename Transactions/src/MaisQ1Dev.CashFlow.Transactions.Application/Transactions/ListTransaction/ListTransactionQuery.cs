using FluentValidation;
using MaisQ1Dev.CashFlow.Transactions.Application.Transactions.Common;
using MaisQ1Dev.Libs.Domain;
using MediatR;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Transactions.ListTransaction;

public sealed record ListTransactionQuery(
    Guid CompanyId,
    DateTime StartDate,
    DateTime EndDate,
    int PageNumber,
    int PageSize) : IRequest<PagedResult<IEnumerable<TransactionResponse>>>;

public class ListTransactionQueryValidator : AbstractValidator<ListTransactionQuery>
{
    public ListTransactionQueryValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty().WithErrorCode("Query.CompanyId").WithMessage("Company Id can't be empty");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithErrorCode("Query.StartDate").WithMessage("Start Date can't be empty");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithErrorCode("Query.EndDate").WithMessage("End Date can't be empty")
            .GreaterThanOrEqualTo(x => x.StartDate)
                .WithErrorCode("Query.EndDate")
                .WithMessage("End Date must be greater than or equal to Start Date");

        RuleFor(x => x.PageNumber)
            .NotEmpty().WithErrorCode("Query.PageNumber").WithMessage("Page Number can't be empty")
            .GreaterThan(0).WithErrorCode("Query.PageNumber").WithMessage("Page Number must be greater than 0");

        RuleFor(x => x.PageSize)
            .NotEmpty().WithErrorCode("Query.PageSize").WithMessage("Page Size can't be empty")
            .GreaterThan(0).WithErrorCode("Query.PageSize").WithMessage("Page Size must be greater than 0");
    }
}