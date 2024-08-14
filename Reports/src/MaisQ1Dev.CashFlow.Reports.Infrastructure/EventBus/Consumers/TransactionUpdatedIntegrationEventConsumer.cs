using MaisQ1Dev.CashFlow.Reports.Application.Transactions.UpdateTransaction;
using MaisQ1Dev.Libs.Domain.Logging;
using MaisQ1Dev.Libs.IntegrationEvents.Transaction;
using MassTransit;
using MediatR;

namespace MaisQ1Dev.CashFlow.Reports.Infrastructure.EventBus.Consumers;

public sealed class TransactionUpdatedIntegrationEventConsumer : IConsumer<TransactionUpdatedIntegrationEvent>
{
    private readonly ISender _sender;
    private readonly ILoggerMQ1Dev<TransactionUpdatedIntegrationEventConsumer> _logger;

    public TransactionUpdatedIntegrationEventConsumer(
        ISender sender,
        ILoggerMQ1Dev<TransactionUpdatedIntegrationEventConsumer> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TransactionUpdatedIntegrationEvent> context)
    {
        _logger.LogInformation("Reading the message to integrate the update for transaction {TransactionId}. [Transaction:{@Transaction}]",
            context.Message.TransactionId,
            context.Message);

        var updateTransactionCommand = new UpdateTransactionCommand(
            context.Message.TransactionId,
            context.Message.CompanyId,
            context.Message.Date,
            context.Message.Amount,
            context.Message.Description);

        var result = await _sender.Send(updateTransactionCommand, default);
        if (result.IsFailure)
        {
            _logger.LogError("Error updating transaction {TransactionId}", context.Message.TransactionId);
            return;
        }

        _logger.LogInformation("Successful integration of the update for transaction {TransactionId}", context.Message.TransactionId);
    }
}
