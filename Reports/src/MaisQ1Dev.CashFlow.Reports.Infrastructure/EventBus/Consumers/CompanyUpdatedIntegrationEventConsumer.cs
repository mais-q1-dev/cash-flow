using MaisQ1Dev.CashFlow.Reports.Application.Companies.UpdateCompany;
using MaisQ1Dev.Libs.Domain.Logging;
using MaisQ1Dev.Libs.IntegrationEvents.Companies;
using MassTransit;
using MediatR;

namespace MaisQ1Dev.CashFlow.Reports.Infrastructure.EventBus.Consumers;

public sealed class CompanyUpdatedIntegrationEventConsumer : IConsumer<CompanyUpdatedIntegrationEvent>
{
    private readonly ISender _sender;
    private readonly ILoggerMQ1Dev<CompanyUpdatedIntegrationEventConsumer> _logger;

    public CompanyUpdatedIntegrationEventConsumer(
        ISender sender,
        ILoggerMQ1Dev<CompanyUpdatedIntegrationEventConsumer> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CompanyUpdatedIntegrationEvent> context)
    {
        _logger.LogInformation("Reading the message to integrate the update for company {CompanyId}. [Company:{@Company}]",
            context.Message.CompanyId,
            context.Message);

        var updateCompanyCommand = new UpdateCompanyCommand(
            context.Message.CompanyId,
            context.Message.Name,
            context.Message.Email);

        var result = await _sender.Send(updateCompanyCommand, default);
        if (result.IsFailure)
        {
            _logger.LogError("Error updating company {CompanyId}", context.Message.CompanyId);
            return;
        }

        _logger.LogInformation("Successful integration of the update for company {CompanyId}", context.Message.CompanyId);
    }
}
