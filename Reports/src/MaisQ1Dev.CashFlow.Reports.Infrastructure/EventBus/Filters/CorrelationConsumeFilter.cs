using MaisQ1Dev.Libs.Domain.Tracing;
using MassTransit;
using Serilog.Events;

namespace MaisQ1Dev.CashFlow.Reports.Infrastructure.EventBus.Filters;

public class CorrelationConsumeFilter<T> : IFilter<ConsumeContext<T>> where T : class
{
    public Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        var correlationIdHeader = context.CorrelationId;

        if (correlationIdHeader.HasValue)
        {
            var correlationId = correlationIdHeader.Value;

            Serilog.Context.LogContext.PushProperty("CorrelationId", new ScalarValue(correlationId));
            AsyncStorage<Correlation>.Store(new Correlation { Id = correlationId });
        }

        return next.Send(context);
    }

    public void Probe(ProbeContext context)
    { }
}