using MaisQ1Dev.Libs.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MaisQ1Dev.CashFlow.Reports.Application.Abstractions.Behavior;

public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        => _logger = logger;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation(
                "Starting request {@RequestName}: {@Request}",
                typeof(TRequest).Name,
                request);

            var result = await next();

            if (result.IsFailure)
                _logger.LogError(
                    "Request failure {@RequestName} {@Erro}",
                    typeof(TRequest).Name,
                    result.Errors.First());

            _logger.LogInformation(
                "Completed request {@RequestName}",
                typeof(TRequest).Name);

            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(
                e,
                "Request {@RequestName} failure with exception: {@ExceptionMessage}",
                typeof(TRequest).Name,
                e.Message);

            throw;
        }
    }
}