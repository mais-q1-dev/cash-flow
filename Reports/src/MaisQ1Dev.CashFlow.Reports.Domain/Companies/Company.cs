using CommunityToolkit.Diagnostics;
using MaisQ1Dev.Libs.Domain.Entities;

namespace MaisQ1Dev.CashFlow.Reports.Domain.Companies;

public class Company : Entity
{
    private Company(
        Guid id,
        string name,
        Email email)
    {
        Id = id;
        Name = name;
        Email = email;
        Balance = 0.00m;
    }

    protected Company() { }

    public string Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public decimal Balance { get; private set; }

    public static Company Create(
        Guid id,
        string name,
        string emailAddress)
    {
        Guard.IsNotDefault(id, nameof(id));
        Guard.IsNotNullOrWhiteSpace(name, nameof(name));
        Guard.IsNotNullOrWhiteSpace(emailAddress, nameof(emailAddress));

        var email = Email.Create(emailAddress);
        var company = new Company(id, name, email);

        return company;
    }

    public void Update(
        string name,
        string emailAddress)
    {
        Guard.IsNotNullOrWhiteSpace(name, nameof(name));
        Guard.IsNotNullOrWhiteSpace(emailAddress, nameof(emailAddress));

        Name = name;
        Email = Email.Create(emailAddress);
    }
}
