using MaisQ1Dev.CashFlow.Reports.Domain.Companies;

namespace MaisQ1Dev.CashFlow.Reports.Application.Tests.Utils;

public static class CompanyMother
{
    public static readonly Company RitaESaraEletronica = Create(
        Guid.Parse("3f5ff118-d3fd-42d5-b69a-91c4008b3ec5"),
        "Rita e Sara Eletrônica Ltda",
        "faleconosco@ritaesaraeletronicaltda.com.br");
    public static readonly Company OtavioELuciaConstrucoes = Create(
        Guid.Parse("93cf443f-11b9-4bb4-a013-db6485a6f68c"),
        "Otávio e Lúcia Construções Ltda",
        "marketing@otavioeluciaconstrucoesltda.com.br");

    public static Company Create(
        Guid? id = null,
        string? name = null,
        string? email = null)
        => Company.Create(
            id ?? Guid.NewGuid(),
            name ?? "Company Name",
            email ?? "finance@company.com");
}