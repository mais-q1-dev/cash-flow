using MaisQ1Dev.CashFlow.Transactions.Domain.Companies.DomainEvents;
using MaisQ1Dev.Libs.IntegrationEvents.Companies;
using MaisQ1Dev.Libs.IntegrationEvents.EventBus;
using MediatR;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Companies.DomainEvents;

public sealed class CompanyCreatedDomainEventHandler : INotificationHandler<CompanyCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public CompanyCreatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(CompanyCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var @event = new CompanyCreatedIntegrationEvent(
            notification.CompanyId,
            notification.Name,
            notification.Email);

        await _eventBus.PublishAsync(@event);
    }
}
