using MaisQ1Dev.CashFlow.Transactions.Domain.Transactions.DomainEvents;
using MaisQ1Dev.Libs.IntegrationEvents.EventBus;
using MaisQ1Dev.Libs.IntegrationEvents.Transaction;
using MediatR;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Transactions.DomainEvents;

public sealed class TransactionCreatedDomainEventHandler : INotificationHandler<TransactionCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public TransactionCreatedDomainEventHandler(IEventBus eventBus)
        => _eventBus = eventBus;

    public async Task Handle(TransactionCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var @event = new TransactionCreatedIntegrationEvent(
            notification.TransactionId,
            notification.CompanyId,
            notification.Date,
            notification.Amount,
            notification.Description);

        await _eventBus.PublishAsync(@event);
    }
}
