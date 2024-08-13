using FluentValidation;
using MaisQ1Dev.CashFlow.Transactions.Domain.Transactions;
using MaisQ1Dev.Libs.Domain;
using MediatR;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Transactions.CreateTransaction;

public sealed record CreateTransactionCommand(
    Guid CompanyId,
    ETransactionType Type,
    DateTime Date,
    decimal Amount,
    string? Description) : IRequest<Result<Guid>>;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty().WithErrorCode("Command.CompanyId").WithMessage("Company Id can't be empty");

        RuleFor(x => x.Type)
            .IsInEnum().WithErrorCode("Command.Type").WithMessage("Type invalid");

        RuleFor(x => x.Date)
            .NotEmpty().WithErrorCode("Command.Date").WithMessage("Date can't be empty");

        RuleFor(x => x.Amount)
            .NotEmpty().WithErrorCode("Command.Amount").WithMessage("Amount can't be empty")
            .GreaterThan(0).WithErrorCode("Command.Amount").WithMessage("Amount must be greater than 0");

        RuleFor(x => x.Description)
            .MaximumLength(200).WithErrorCode("Command.Description").WithMessage("Description must be max 200 characters");
    }
}