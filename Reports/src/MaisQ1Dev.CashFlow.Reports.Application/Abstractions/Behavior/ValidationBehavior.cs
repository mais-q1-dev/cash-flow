using FluentValidation;
using MaisQ1Dev.Libs.Domain;
using MaisQ1Dev.Libs.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MaisQ1Dev.CashFlow.Reports.Application.Abstractions.Behavior;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<LoggingBehavior<TRequest, TResponse>> logger)
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
            _logger.LogInformation(
                "There is no validator registered for request {@RequestName}",
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
            throw new BusinessValidationException(validationErrors);

        return await next();
    }
}