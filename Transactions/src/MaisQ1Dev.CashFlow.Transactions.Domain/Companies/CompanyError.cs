using MaisQ1Dev.Libs.Domain;

namespace MaisQ1Dev.CashFlow.Transactions.Domain.Companies;

public static class CompanyError
{
    public static Error NotFound => new("Company.NotFound", "Company not found");
}
