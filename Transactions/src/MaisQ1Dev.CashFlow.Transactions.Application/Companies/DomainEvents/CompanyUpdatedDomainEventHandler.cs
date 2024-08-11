using MaisQ1Dev.CashFlow.Transactions.Domain.Companies.DomainEvents;
using MaisQ1Dev.Libs.IntegrationEvents.Companies;
using MaisQ1Dev.Libs.IntegrationEvents.EventBus;
using MediatR;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Companies.DomainEvents;

public sealed class CompanyUpdatedDomainEventHandler : INotificationHandler<CompanyUpdatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public CompanyUpdatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(CompanyUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var @event = new CompanyUpdatedIntegrationEvent(
            notification.CompanyId,
            notification.Name,
            notification.Email);

        await _eventBus.PublishAsync(@event);
    }
}