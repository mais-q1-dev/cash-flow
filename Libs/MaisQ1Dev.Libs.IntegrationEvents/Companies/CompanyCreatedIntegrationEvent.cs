using MaisQ1Dev.Libs.IntegrationEvents.EventBus;

namespace MaisQ1Dev.Libs.IntegrationEvents.Companies;

public sealed record CompanyCreatedIntegrationEvent(
    Guid CompanyId,
    string Name,
    string Email) : IntegrationEvent;