using FluentValidation;
using MaisQ1Dev.Libs.Domain;
using MaisQ1Dev.Libs.Domain.Exceptions;
using MaisQ1Dev.Libs.Domain.Logging;
using MediatR;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Abstractions.Behavior;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILoggerMQ1Dev<LoggingBehavior<TRequest, TResponse>> _logger;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators,
        ILoggerMQ1Dev<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            if (typeof(TRequest).Name.EndsWith("Command", StringComparison.OrdinalIgnoreCase))
                _logger.LogWarning(
                    "No validators found for the {RequestName} request",
                    typeof(TRequest).Name);

            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationErrors = _validators
            .Select(validator => validator.Validate(context))
            .Where(validationResult => validationResult.Errors.Any())
            .SelectMany(validationResult => validationResult.Errors)
            .Select(validationFailure => new Error(validationFailure.ErrorCode, validationFailure.ErrorMessage))
            .ToList();

        if (validationErrors.Count != 0)
        {
            _logger.LogError(
                "Errors occurred during the {RequestName} request validation",
                typeof(TRequest).Name);

            throw new BusinessValidationException(validationErrors);
        }

        return await next();
    }
}