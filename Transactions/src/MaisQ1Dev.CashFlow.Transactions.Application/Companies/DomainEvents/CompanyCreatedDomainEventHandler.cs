using MaisQ1Dev.CashFlow.Transactions.Domain.Companies.DomainEvents;
using MaisQ1Dev.Libs.Domain.Logging;
using MaisQ1Dev.Libs.IntegrationEvents.Companies;
using MaisQ1Dev.Libs.IntegrationEvents.EventBus;
using MediatR;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Companies.DomainEvents;

public sealed class CompanyCreatedDomainEventHandler : INotificationHandler<CompanyCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;
    private readonly ILoggerMQ1Dev<CompanyCreatedDomainEventHandler> _logger;

    public CompanyCreatedDomainEventHandler(
        IEventBus eventBus,
        ILoggerMQ1Dev<CompanyCreatedDomainEventHandler> logger)
    {
        _eventBus = eventBus;
        _logger = logger;
    }

    public async Task Handle(CompanyCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var @event = new CompanyCreatedIntegrationEvent(
            notification.CompanyId,
            notification.Name,
            notification.Email);

        _logger.LogInformation("CompanyCreatedIntegrationEvent: {Event}", @event);
        await _eventBus.PublishAsync(@event);
        _logger.LogInformation("CompanyCreatedIntegrationEvent published");
    }
}
