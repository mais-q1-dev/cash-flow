using MaisQ1Dev.Libs.IntegrationEvents.EventBus;
using MassTransit;

namespace MaisQ1Dev.CashFlow.Reports.Infrastructure.EventBus;

public class RabbitMQEventBus : IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMessageScheduler _messageScheduler;

    public RabbitMQEventBus(
        IPublishEndpoint publishEndpoint,
        IMessageScheduler messageScheduler)
    {
        _publishEndpoint = publishEndpoint;
        _messageScheduler = messageScheduler;
    }

    public async Task PublishAsync<T>(T @event) where T : IntegrationEvent
        => await _publishEndpoint.Publish(@event);

    public async Task SchedulePublishAsync<T>(T @event, TimeSpan delay) where T : IntegrationEvent
        => await _messageScheduler.SchedulePublish(delay, @event);
}