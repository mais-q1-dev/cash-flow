using MaisQ1Dev.Libs.Domain.Tracing;
using MassTransit;

namespace MaisQ1Dev.CashFlow.Transactions.Infrastructure.EventBus.Filters;

public class CorrelationPublishFilter<T> : IFilter<PublishContext<T>> where T : class
{

    public Task Send(PublishContext<T> context, IPipe<PublishContext<T>> next)
    {
        var correlation = AsyncStorage<Correlation>.Retrieve();

        if (correlation is not null)
        {
            context.CorrelationId = Guid.Parse(correlation.Id.ToString()!);
        }

        return next.Send(context);
    }

    public void Probe(ProbeContext context)
    { }
}