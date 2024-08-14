using MaisQ1Dev.CashFlow.Reports.Application.Companies.CreateCompany;
using MaisQ1Dev.Libs.Domain.Logging;
using MaisQ1Dev.Libs.IntegrationEvents.Companies;
using MassTransit;
using MediatR;

namespace MaisQ1Dev.CashFlow.Reports.Infrastructure.EventBus.Consumers;

public sealed class CompanyCreatedIntegrationEventConsumer : IConsumer<CompanyCreatedIntegrationEvent>
{
    private readonly ISender _sender;
    private readonly ILoggerMQ1Dev<CompanyCreatedIntegrationEventConsumer> _logger;

    public CompanyCreatedIntegrationEventConsumer(
        ISender sender,
        ILoggerMQ1Dev<CompanyCreatedIntegrationEventConsumer> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CompanyCreatedIntegrationEvent> context)
    {
        _logger.LogInformation("Reading the message to integrate the company {CompanyId}. [Company:{@Company}]",
            context.Message.CompanyId,
            context.Message);

        var createCompanyCommand = new CreateCompanyCommand(
            context.Message.CompanyId,
            context.Message.Name,
            context.Message.Email);

        var result = await _sender.Send(createCompanyCommand, default);
        if (result.IsFailure)
        {
            _logger.LogError("Error creating company {CompanyId}", context.Message.CompanyId);
            return;
        }

        _logger.LogInformation("Company {CompanyId} integrated successfully", context.Message.CompanyId);
    }
}

