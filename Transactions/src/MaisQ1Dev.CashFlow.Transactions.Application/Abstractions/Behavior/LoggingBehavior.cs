using MaisQ1Dev.Libs.Domain;
using MaisQ1Dev.Libs.Domain.Logging;
using MaisQ1Dev.Libs.Domain.Tracing;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Abstractions.Behavior;

public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly ILoggerMQ1Dev<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILoggerMQ1Dev<LoggingBehavior<TRequest, TResponse>> logger)
        => _logger = logger;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation(
                "Starting request {RequestName}: {@Request}",
                typeof(TRequest).Name,
                request);

            var result = await next();

            if (result.IsFailure)
                _logger.LogError(
                    "Request {RequestName} failure with {@Erro}",
                    typeof(TRequest).Name,
                    result.Errors.ToList());

            _logger.LogInformation(
                "Completed request {RequestName}",
                typeof(TRequest).Name);


            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(
                e,
                "Request {RequestName} failure",
                typeof(TRequest).Name);

            throw;
        }
    }
}