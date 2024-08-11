using MaisQ1Dev.Libs.Domain.Entities;

namespace MaisQ1Dev.CashFlow.Transactions.Domain.Companies.DomainEvents;

public sealed record CompanyCreatedDomainEvent(
    Guid CompanyId,
    string Name,
    string Email) : IDomainEvent;