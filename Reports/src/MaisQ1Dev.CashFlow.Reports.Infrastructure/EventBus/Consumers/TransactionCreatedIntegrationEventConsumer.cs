using MaisQ1Dev.CashFlow.Reports.Application.Transactions.CreateTransaction;
using MaisQ1Dev.Libs.Domain.Logging;
using MaisQ1Dev.Libs.IntegrationEvents.Transaction;
using MassTransit;
using MediatR;

namespace MaisQ1Dev.CashFlow.Reports.Infrastructure.EventBus.Consumers;

public sealed class TransactionCreatedIntegrationEventConsumer : IConsumer<TransactionCreatedIntegrationEvent>
{
    private readonly ISender _sender;
    private readonly ILoggerMQ1Dev<TransactionCreatedIntegrationEventConsumer> _logger;

    public TransactionCreatedIntegrationEventConsumer(
        ISender sender,
        ILoggerMQ1Dev<TransactionCreatedIntegrationEventConsumer> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TransactionCreatedIntegrationEvent> context)
    {
        _logger.LogInformation("Reading the message to integrate the transaction {TransactionId}. [Transaction:{@Transaction}]",
            context.Message.TransactionId,
            context.Message);

        var transaction = new CreateTransactionCommand(
            context.Message.TransactionId,
            context.Message.CompanyId,
            context.Message.Date,
            context.Message.Amount,
            context.Message.Description);

        var result = await _sender.Send(transaction, default);
        if (result.IsFailure)
        {
            _logger.LogError("Error creating transaction {TransactionId}", context.Message.TransactionId);
            return;
        }

        _logger.LogInformation("Transaction {TransactionId} integrated successfully", context.Message.TransactionId);
    }
}
