using MaisQ1Dev.CashFlow.Reports.Application.Companies.UpdateCompany;
using MaisQ1Dev.Libs.IntegrationEvents.Companies;
using MassTransit;
using MediatR;

namespace MaisQ1Dev.CashFlow.Reports.Infrastructure.EventBus.Consumers;

public sealed class CompanyUpdatedIntegrationEventConsumer : IConsumer<CompanyUpdatedIntegrationEvent>
{
    private readonly ISender _sender;

    public CompanyUpdatedIntegrationEventConsumer(ISender sender)
        => _sender = sender;

    public async Task Consume(ConsumeContext<CompanyUpdatedIntegrationEvent> context)
    {
        var updateCompanyCommand = new UpdateCompanyCommand(
            context.Message.CompanyId,
            context.Message.Name,
            context.Message.Email);

        var result = await _sender.Send(updateCompanyCommand, default);
        if (result.IsFailure)
        {
            //_logger.LogError("Error updating company [{CompanyId}]", context
            //    .Message.CompanyId);
            return;
        }

        //_logger.LogInformation("Company [{CompanyId}] updated", context.Message.CompanyId);
    }
}
