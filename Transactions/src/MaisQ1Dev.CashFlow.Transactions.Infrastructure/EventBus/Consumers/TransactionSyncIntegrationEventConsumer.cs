using MaisQ1Dev.CashFlow.Transactions.Application.Transactions.SyncTransaction;
using MaisQ1Dev.Libs.IntegrationEvents.Transaction;
using MassTransit;
using MediatR;

namespace MaisQ1Dev.CashFlow.Transactions.Infrastructure.EventBus.Consumers;

public sealed class TransactionSyncIntegrationEventConsumer : IConsumer<TransactionSyncIntegrationEvent>
{
    private readonly ISender _sender;

    public TransactionSyncIntegrationEventConsumer(ISender sender)
        => _sender = sender;

    public async Task Consume(ConsumeContext<TransactionSyncIntegrationEvent> context)
    {
        var syncTransactionCommand = new SyncTransactionCommand(context.Message.TransactionId);

        var result = await _sender.Send(syncTransactionCommand, default);
        if (result.IsFailure)
        {
            //_logger.LogError("Error synchronizing transaction [{TransactionId}]", context.Message.TransactionId);
            return;
        }

        //_logger.LogInformation("Transaction [{TransactionId}] synchronized", context.Message.TransactionId);
    }
}
