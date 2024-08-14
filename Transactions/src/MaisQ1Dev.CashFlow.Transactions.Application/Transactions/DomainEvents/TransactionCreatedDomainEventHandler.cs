using MaisQ1Dev.CashFlow.Transactions.Domain.Transactions.DomainEvents;
using MaisQ1Dev.Libs.Domain.Logging;
using MaisQ1Dev.Libs.IntegrationEvents.EventBus;
using MaisQ1Dev.Libs.IntegrationEvents.Transaction;
using MediatR;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Transactions.DomainEvents;

public sealed class TransactionCreatedDomainEventHandler : INotificationHandler<TransactionCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;
    private readonly ILoggerMQ1Dev<TransactionCreatedDomainEventHandler> _logger;

    public TransactionCreatedDomainEventHandler(
        IEventBus eventBus,
        ILoggerMQ1Dev<TransactionCreatedDomainEventHandler> logger)
    {
        _eventBus = eventBus;
        _logger = logger;
    }

    public async Task Handle(TransactionCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var @event = new TransactionCreatedIntegrationEvent(
            notification.TransactionId,
            notification.CompanyId,
            notification.Date,
            notification.Amount,
            notification.Description);

        _logger.LogInformation("TransactionCreatedIntegrationEvent: {Event}", @event);
        await _eventBus.PublishAsync(@event);
        _logger.LogInformation("TransactionCreatedIntegrationEvent published");
    }
}
