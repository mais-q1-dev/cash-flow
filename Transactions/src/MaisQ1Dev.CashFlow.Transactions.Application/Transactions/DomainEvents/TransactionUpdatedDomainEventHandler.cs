﻿using MaisQ1Dev.CashFlow.Transactions.Domain.Transactions.DomainEvents;
using MaisQ1Dev.Libs.Domain.Logging;
using MaisQ1Dev.Libs.IntegrationEvents.EventBus;
using MaisQ1Dev.Libs.IntegrationEvents.Transaction;
using MediatR;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Transactions.DomainEvents;

public sealed class TransactionUpdatedDomainEventHandler : INotificationHandler<TransactionUpdatedDomainEvent>
{
    private readonly IEventBus _eventBus;
    private readonly ILoggerMQ1Dev<TransactionUpdatedDomainEventHandler> _logger;

    public TransactionUpdatedDomainEventHandler(
        IEventBus eventBus,
        ILoggerMQ1Dev<TransactionUpdatedDomainEventHandler> logger)
    {
        _eventBus = eventBus;
        _logger = logger;
    }

    public async Task Handle(TransactionUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var @event = new TransactionUpdatedIntegrationEvent(
            notification.TransactionId,
            notification.CompanyId,
            notification.Date,
            notification.Amount,
            notification.Description);

        _logger.LogInformation("TransactionUpdatedIntegrationEvent: {Event}", @event);
        await _eventBus.PublishAsync(@event);
        _logger.LogInformation("TransactionUpdatedIntegrationEvent published");
    }
}
