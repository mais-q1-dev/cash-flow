using MaisQ1Dev.CashFlow.Transactions.Application.Transactions.SyncTransaction;
using MaisQ1Dev.Libs.Domain.Logging;
using MaisQ1Dev.Libs.IntegrationEvents.Transaction;
using MassTransit;
using MediatR;

namespace MaisQ1Dev.CashFlow.Transactions.Infrastructure.EventBus.Consumers;

public sealed class TransactionSyncIntegrationEventConsumer : IConsumer<TransactionSyncIntegrationEvent>
{
    private readonly ISender _sender;
    private readonly ILoggerMQ1Dev<TransactionSyncIntegrationEventConsumer> _logger;

    public TransactionSyncIntegrationEventConsumer(
        ISender sender,
        ILoggerMQ1Dev<TransactionSyncIntegrationEventConsumer> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TransactionSyncIntegrationEvent> context)
    {
        _logger.LogInformation("Synchronizing transaction {TransactionId}. [Transaction:{@Transaction}]", 
            context.Message.TransactionId,
            context.Message);

        var syncTransactionCommand = new SyncTransactionCommand(context.Message.TransactionId);

        var result = await _sender.Send(syncTransactionCommand, default);
        if (result.IsFailure)
        {
            _logger.LogError(
                "Error synchronizing transaction {TransactionId}", 
                context.Message.TransactionId);
            
            return;
        }

        _logger.LogInformation("Transaction {TransactionId} synchronized", context.Message.TransactionId);
    }
}
