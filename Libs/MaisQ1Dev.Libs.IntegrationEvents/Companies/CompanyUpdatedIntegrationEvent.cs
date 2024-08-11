using MaisQ1Dev.Libs.IntegrationEvents.EventBus;

namespace MaisQ1Dev.Libs.IntegrationEvents.Companies;

public sealed record CompanyUpdatedIntegrationEvent(
    Guid CompanyId,
    string Name,
    string Email) : IntegrationEvent;