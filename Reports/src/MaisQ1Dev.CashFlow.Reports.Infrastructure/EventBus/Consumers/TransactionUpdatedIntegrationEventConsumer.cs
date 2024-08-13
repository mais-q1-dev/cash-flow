using MaisQ1Dev.CashFlow.Reports.Application.Transactions.UpdateTransaction;
using MaisQ1Dev.Libs.IntegrationEvents.Transaction;
using MassTransit;
using MediatR;

namespace MaisQ1Dev.CashFlow.Reports.Infrastructure.EventBus.Consumers;

public sealed class TransactionUpdatedIntegrationEventConsumer : IConsumer<TransactionUpdatedIntegrationEvent>
{
    private readonly ISender _sender;

    public TransactionUpdatedIntegrationEventConsumer(ISender sender)
        => _sender = sender;

    public async Task Consume(ConsumeContext<TransactionUpdatedIntegrationEvent> context)
    {
        var updateTransactionCommand = new UpdateTransactionCommand(
            context.Message.TransactionId,
            context.Message.CompanyId,
            context.Message.Date,
            context.Message.Amount,
            context.Message.Description);

        var result = await _sender.Send(updateTransactionCommand, default);
        if (result.IsFailure)
        {
            //_logger.LogError("Error synchronizing transaction [{TransactionId}]", context.Message.TransactionId);
            return;
        }

        //_logger.LogInformation("Transaction [{TransactionId}] synchronized", context.Message.TransactionId);
    }
}
