using MaisQ1Dev.CashFlow.Reports.Domain.Transactions;

namespace MaisQ1Dev.CashFlow.Reports.Application.Tests.Utils;

public static class TransactionMother
{
    public static readonly Transaction Salary = Create(
        Guid.Parse("ce321c42-e709-4bb2-a717-de94136382bf"),
        CompanyMother.RitaESaraEletronica.Id,
        DateTime.UtcNow,
        5_000m,
        "Salary");

    public static readonly Transaction InternetSubscription = Create(
        Guid.Parse("4df0b28b-92f2-4ed6-aa71-d3e722df7aa5"),
        CompanyMother.RitaESaraEletronica.Id,
        DateTime.UtcNow,
        -39.90m,
        "Internet Subscription");

    public static readonly Transaction InvestmentIncome = Create(
        Guid.Parse("3c820fa2-48a9-46af-ae80-8947de478421"),
        CompanyMother.OtavioELuciaConstrucoes.Id,
        DateTime.UtcNow,
        3_000m,
        "Investment Income");

    public static Transaction Create(
        Guid? id = null,
        Guid? companyId = null,
        DateTime? date = null,
        decimal? amount = null,
        string? description = null)
        => Transaction.Create(
            id ?? Guid.NewGuid(),
            companyId ?? CompanyMother.RitaESaraEletronica.Id,
            date ?? DateTime.UtcNow,
            amount ?? 100m,
            description);
}
