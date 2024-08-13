namespace MaisQ1Dev.CashFlow.Transactions.Application.Tests.Utils;

public static class CompanyMother
{
    public static readonly Company RitaESaraEletronica = Create("Rita e Sara Eletrônica Ltda", "faleconosco@ritaesaraeletronicaltda.com.br");
    public static readonly Company OtavioELuciaConstrucoes = Create("Otávio e Lúcia Construções Ltda", "marketing@otavioeluciaconstrucoesltda.com.br");

    public static Company Create(string name, string email)
        => Company.Create(name, email);
}
