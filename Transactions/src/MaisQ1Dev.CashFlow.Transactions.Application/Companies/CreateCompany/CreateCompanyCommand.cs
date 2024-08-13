using FluentValidation;
using MaisQ1Dev.Libs.Domain;
using MediatR;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Companies.CreateCompany;

public sealed record CreateCompanyCommand(string Name, string Email) : IRequest<Result<Guid>>;

public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithErrorCode("Command.Name").WithMessage("Name can't be empty")
            .MaximumLength(200).WithErrorCode("Command.Name").WithMessage("Name must be max 200 characters");

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode("Command.Email").WithMessage("E-mail can't be empty")
            .Matches(@"^(?=.{1,255}$)([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})$").WithErrorCode("Command.Email").WithMessage("E-mail invalid");
    }
}