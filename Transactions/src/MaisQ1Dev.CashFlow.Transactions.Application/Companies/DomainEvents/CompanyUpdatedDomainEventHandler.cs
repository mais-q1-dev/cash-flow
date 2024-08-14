using MaisQ1Dev.CashFlow.Transactions.Domain.Companies.DomainEvents;
using MaisQ1Dev.Libs.Domain.Logging;
using MaisQ1Dev.Libs.IntegrationEvents.Companies;
using MaisQ1Dev.Libs.IntegrationEvents.EventBus;
using MediatR;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Companies.DomainEvents;

public sealed class CompanyUpdatedDomainEventHandler : INotificationHandler<CompanyUpdatedDomainEvent>
{
    private readonly IEventBus _eventBus;
    private readonly ILoggerMQ1Dev<CompanyUpdatedDomainEventHandler> _logger;

    public CompanyUpdatedDomainEventHandler(
        IEventBus eventBus,
        ILoggerMQ1Dev<CompanyUpdatedDomainEventHandler> logger)
    {
        _eventBus = eventBus;
        _logger = logger;
    }

    public async Task Handle(CompanyUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var @event = new CompanyUpdatedIntegrationEvent(
            notification.CompanyId,
            notification.Name,
            notification.Email);

        _logger.LogInformation("CompanyUpdatedIntegrationEvent: {Event}", @event);
        await _eventBus.PublishAsync(@event);
        _logger.LogInformation("CompanyUpdatedIntegrationEvent published");
    }
}