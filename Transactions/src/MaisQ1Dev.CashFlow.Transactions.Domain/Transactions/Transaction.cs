using CommunityToolkit.Diagnostics;
using MaisQ1Dev.CashFlow.Transactions.Domain.Companies;
using MaisQ1Dev.CashFlow.Transactions.Domain.Transactions.DomainEvents;
using MaisQ1Dev.Libs.Domain.Entities;

namespace MaisQ1Dev.CashFlow.Transactions.Domain.Transactions;

public class Transaction : Entity
{
    private Transaction(
        Guid companyId,
        ETransactionType type,
        DateTime date,
        decimal amount,
        string? description)
    {
        CompanyId = companyId;
        Date = date;
        Type = type;
        Amount = Math.Abs(amount);
        Description = description;
        SyncStatus = ETransactionSyncStatus.Pending;

        if (Type == ETransactionType.Expense)
            Amount *= -1;
    }

    protected Transaction() { }

    public Guid CompanyId { get; private set; }
    public virtual Company Company { get; init; } = null!;
    public ETransactionType Type { get; private set; }
    public DateTime Date { get; private set; }
    public decimal Amount { get; private set; }
    public string? Description { get; private set; }
    public ETransactionSyncStatus SyncStatus { get; private set; }

    public static Transaction Create(
        Guid companyId,
        ETransactionType type,
        DateTime date,
        decimal amount,
        string? description)
    {
        Guard.IsNotDefault(companyId, nameof(companyId));
        Guard.IsNotDefault(date, nameof(date));
        Guard.IsGreaterThan(amount, 0, nameof(amount));

        var transaction = new Transaction(companyId, type, date, amount, description);
        transaction.RaiseDomainEvent(new TransactionCreatedDomainEvent(
            transaction.Id,
            transaction.CompanyId,
            transaction.Date,
            transaction.Amount,
            transaction.Description));

        return transaction;
    }

    public void Update(
        ETransactionType type,
        DateTime date,
        decimal amount,
        string? description)
    {
        Guard.IsNotDefault(date, nameof(date));
        Guard.IsGreaterThan(amount, 0, nameof(amount));

        Date = date;
        Type = type;
        Amount = Math.Abs(amount);
        Description = description;
        SyncStatus = ETransactionSyncStatus.Pending;

        if (Type == ETransactionType.Expense)
            Amount *= -1;

        RaiseDomainEvent(new TransactionUpdatedDomainEvent(
            Id,
            CompanyId,
            Date,
            Amount,
            Description));
    }

    public void Sync() => SyncStatus = ETransactionSyncStatus.Synced;
}