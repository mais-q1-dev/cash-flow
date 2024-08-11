namespace MaisQ1Dev.CashFlow.Transactions.Api.Middlewares;

public class LoggingContextMiddleware : IMiddleware
{
    private readonly ILogger<LoggingContextMiddleware> _logger;

    public LoggingContextMiddleware(ILogger<LoggingContextMiddleware> logger)
        => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var queryStringValues = string.Empty;
        if (context.Request.QueryString.HasValue)
            queryStringValues = context.Request.QueryString.Value;

        var requestBody = await ReadBodyFromRequest(context.Request);

        _logger.LogInformation("[{@TraceId}] Request in [{@RequestMethod}] {@RequestPath} with params: {@params}",
            context.TraceIdentifier,
            context.Request.Method,
            context.Request.Path,
            string.IsNullOrWhiteSpace(queryStringValues)
                ? requestBody
                : queryStringValues);

        await next(context);
    }

    private async Task<string> ReadBodyFromRequest(HttpRequest request)
    {
        // Ensure the request's body can be read multiple times (for the next middlewares in the pipeline).
        request.EnableBuffering();

        using var streamReader = new StreamReader(request.Body, leaveOpen: true);
        var requestBody = await streamReader.ReadToEndAsync();

        // Reset the request's body stream position for next middleware in the pipeline.
        request.Body.Position = 0;
        return requestBody != string.Empty ? requestBody : "no body in request";
    }
}