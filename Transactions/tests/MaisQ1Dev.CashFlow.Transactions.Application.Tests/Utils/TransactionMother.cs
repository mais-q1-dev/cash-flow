namespace MaisQ1Dev.CashFlow.Transactions.Application.Tests.Utils;

public static class TransactionMother
{
    public static readonly Transaction Salary = Create(
        CompanyMother.RitaESaraEletronica.Id,
        ETransactionType.Income,
        DateTime.UtcNow,
        5_000m,
        "Salary");

    public static readonly Transaction InternetSubscription = Create(
        CompanyMother.RitaESaraEletronica.Id,
        ETransactionType.Expense,
        DateTime.UtcNow,
        39.90m,
        "Internet Subscription");

    public static readonly Transaction InvestmentIncome = Create(
        CompanyMother.OtavioELuciaConstrucoes.Id,
        ETransactionType.Income,
        DateTime.UtcNow,
        3_000m,
        "Investment Income");

    public static Transaction Create(
        Guid? companyId = null,
        ETransactionType? type = null,
        DateTime? date = null,
        decimal? amount = null,
        string? description = null)
        => Transaction.Create(
            companyId ?? CompanyMother.RitaESaraEletronica.Id,
            type ?? ETransactionType.Income,
            date ?? DateTime.UtcNow,
            amount ?? 100m,
            description);
}
