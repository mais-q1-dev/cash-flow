using MaisQ1Dev.Libs.Domain.Tracing;
using Serilog.Context;
using Serilog.Events;

namespace MaisQ1Dev.CashFlow.Transactions.Api.Middlewares;

public class CorrelationMiddleware : IMiddleware
{
    private readonly string _correlationIdHeader = "X-Correlation-Id";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var correlationIdHeader = context.Request.Headers[_correlationIdHeader];

        var correlationId = !string.IsNullOrWhiteSpace(correlationIdHeader)
            ? Guid.Parse(correlationIdHeader.ToString())
            : Guid.NewGuid();

        LogContext.PushProperty("CorrelationId", new ScalarValue(correlationId));
        AsyncStorage<Correlation>.Store(new Correlation { Id = correlationId });

        foreach (var query in context.Request.Query)
        {
            LogContext.PushProperty($"RequestQuery.{query.Key}", query.Value.ToString());
        }

        if ((context.Request.ContentType != null) &&
            (context.Request.ContentType.Contains("application/json")))
        {
            context.Request.EnableBuffering();
            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            if (!string.IsNullOrWhiteSpace(body))
                LogContext.PushProperty("RequestBody", body);
        }

        await next(context);
    }
}