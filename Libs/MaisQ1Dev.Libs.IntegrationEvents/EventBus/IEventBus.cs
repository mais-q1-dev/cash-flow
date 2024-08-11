namespace MaisQ1Dev.Libs.IntegrationEvents.EventBus;

public interface IEventBus
{
    Task PublishAsync<T>(T @event) where T : IntegrationEvent;
    Task SchedulePublishAsync<T>(T @event, TimeSpan delay) where T : IntegrationEvent;
}
