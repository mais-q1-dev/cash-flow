﻿using MaisQ1Dev.Libs.Domain.Entities;

namespace MaisQ1Dev.CashFlow.Transactions.Domain.Transactions.DomainEvents;

public sealed record TransactionCreatedDomainEvent(
    Guid TransactionId,
    Guid CompanyId,
    DateTime Date,
    decimal Amount,
    string? Description) : IDomainEvent;
