using MaisQ1Dev.Libs.IntegrationEvents.EventBus;

namespace MaisQ1Dev.Libs.IntegrationEvents.Transaction;

public sealed record TransactionSyncIntegrationEvent(Guid TransactionId) : IntegrationEvent;