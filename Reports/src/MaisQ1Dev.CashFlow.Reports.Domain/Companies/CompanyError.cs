using MaisQ1Dev.Libs.Domain;

namespace MaisQ1Dev.CashFlow.Reports.Domain.Companies;

public static class CompanyError
{
    public static Error NotFound => new("Company.NotFound", "Company not found");
    public static Error AlreadyExists => new("Company.AlreadyExists", "Company id already exists");
}