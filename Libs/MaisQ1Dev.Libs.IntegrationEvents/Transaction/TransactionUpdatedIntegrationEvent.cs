using MaisQ1Dev.Libs.IntegrationEvents.EventBus;

namespace MaisQ1Dev.Libs.IntegrationEvents.Transaction;

public sealed record TransactionUpdatedIntegrationEvent(
    Guid TransactionId,
    Guid CompanyId,
    DateTime Date,
    decimal Amount,
    string? Description) : IntegrationEvent;