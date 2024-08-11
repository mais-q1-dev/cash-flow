using MaisQ1Dev.Libs.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MaisQ1Dev.CashFlow.Transactions.Api.Middlewares;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        => _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var exceptionType = exception.GetType();
        if (exceptionType == typeof(BusinessValidationException))
        {
            var validationProblemDetails = new ProblemDetails
            {
                Type = "https://www.rfc-editor.org/rfc/rfc4918#section-11.2",
                Status = StatusCodes.Status422UnprocessableEntity,
                Title = "Unprocessable Entity",
                Detail = "One or more validation failures have occurred",
                Extensions = new Dictionary<string, object?> { { "errors", ((BusinessValidationException)exception).Errors } }
            };

            httpContext.Response.StatusCode = validationProblemDetails.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(validationProblemDetails, cancellationToken);

            return true;
        }

        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.6.1",
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server error",
            Detail = "An error occurred"
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}