using MaisQ1Dev.CashFlow.Reports.Application.Companies.CreateCompany;
using MaisQ1Dev.Libs.IntegrationEvents.Companies;
using MassTransit;
using MediatR;

namespace MaisQ1Dev.CashFlow.Reports.Infrastructure.EventBus.Consumers;

public sealed class CompanyCreatedIntegrationEventConsumer : IConsumer<CompanyCreatedIntegrationEvent>
{
    private readonly ISender _sender;

    public CompanyCreatedIntegrationEventConsumer(ISender sender)
        => _sender = sender;

    public async Task Consume(ConsumeContext<CompanyCreatedIntegrationEvent> context)
    {
        var createCompanyCommand = new CreateCompanyCommand(
            context.Message.CompanyId,
            context.Message.Name,
            context.Message.Email);

        var result = await _sender.Send(createCompanyCommand, default);
        if (result.IsFailure)
        {
            //_logger.LogError("Error synchronizing transaction [{TransactionId}]", context.Message.TransactionId);
            return;
        }

        //_logger.LogInformation("Transaction [{TransactionId}] synchronized", context.Message.TransactionId);
    }
}

