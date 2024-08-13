using MaisQ1Dev.CashFlow.Reports.Application.Transactions.CreateTransaction;
using MaisQ1Dev.Libs.IntegrationEvents.Transaction;
using MassTransit;
using MediatR;

namespace MaisQ1Dev.CashFlow.Reports.Infrastructure.EventBus.Consumers;

public sealed class TransactionCreatedIntegrationEventConsumer : IConsumer<TransactionCreatedIntegrationEvent>
{
    private readonly ISender _sender;

    public TransactionCreatedIntegrationEventConsumer(ISender sender)
        => _sender = sender;

    public async Task Consume(ConsumeContext<TransactionCreatedIntegrationEvent> context)
    {
        var transaction = new CreateTransactionCommand(
            context.Message.TransactionId,
            context.Message.CompanyId,
            context.Message.Date,
            context.Message.Amount,
            context.Message.Description);

        var result = await _sender.Send(transaction, default);
        if (result.IsFailure)
        {
            //_logger.LogError("Error synchronizing transaction [{TransactionId}]", context.Message.TransactionId);
            return;
        }

        //_logger.LogInformation("Transaction [{TransactionId}] synchronized", context.Message.TransactionId);
    }
}
