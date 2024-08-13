using MaisQ1Dev.CashFlow.Transactions.Domain.Transactions.DomainEvents;
using MaisQ1Dev.Libs.IntegrationEvents.EventBus;
using MaisQ1Dev.Libs.IntegrationEvents.Transaction;
using MediatR;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Transactions.DomainEvents;

public sealed class TransactionUpdatedDomainEventHandler : INotificationHandler<TransactionUpdatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public TransactionUpdatedDomainEventHandler(IEventBus eventBus)
        => _eventBus = eventBus;

    public async Task Handle(TransactionUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var @event = new TransactionUpdatedIntegrationEvent(
            notification.TransactionId,
            notification.CompanyId,
            notification.Date,
            notification.Amount,
            notification.Description);

        await _eventBus.PublishAsync(@event);
    }
}
