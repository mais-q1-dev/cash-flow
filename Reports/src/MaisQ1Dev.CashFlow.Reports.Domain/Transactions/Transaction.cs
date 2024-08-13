using CommunityToolkit.Diagnostics;
using MaisQ1Dev.CashFlow.Reports.Domain.Companies;
using MaisQ1Dev.Libs.Domain.Entities;

namespace MaisQ1Dev.CashFlow.Reports.Domain.Transactions;

public class Transaction : Entity
{
    private Transaction(
        Guid id,
        Guid companyId,
        DateTime date,
        decimal amount,
        string? description)
    {
        Id = id;
        CompanyId = companyId;
        Date = date;
        Amount = amount;
        Description = description;
    }

    protected Transaction() { }

    public Guid CompanyId { get; private set; }
    public virtual Company Company { get; init; } = null!;
    public DateTime Date { get; private set; }
    public decimal Amount { get; private set; }
    public string? Description { get; private set; }

    public static Transaction Create(
        Guid id,
        Guid companyId,
        DateTime date,
        decimal amount,
        string? description)
    {
        Guard.IsNotDefault(companyId, nameof(companyId));
        Guard.IsNotDefault(date, nameof(date));

        var transaction = new Transaction(id, companyId, date, amount, description);
        return transaction;
    }

    public void Update(
        DateTime date,
        decimal amount,
        string? description)
    {
        Guard.IsNotDefault(date, nameof(date));

        Date = date;
        Amount = amount;
        Description = description;
    }
}
