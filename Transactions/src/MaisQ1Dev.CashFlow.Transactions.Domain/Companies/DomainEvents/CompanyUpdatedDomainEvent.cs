using MaisQ1Dev.Libs.Domain.Entities;

namespace MaisQ1Dev.CashFlow.Transactions.Domain.Companies.DomainEvents;

public sealed record CompanyUpdatedDomainEvent(
    Guid CompanyId,
    string Name,
    string Email) : IDomainEvent;