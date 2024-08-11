using CommunityToolkit.Diagnostics;
using MaisQ1Dev.CashFlow.Transactions.Domain.Companies.DomainEvents;
using MaisQ1Dev.Libs.Domain.Entities;

namespace MaisQ1Dev.CashFlow.Transactions.Domain.Companies;

public class Company : Entity
{
    private Company(
        string name,
        Email email)
    {
        Name = name;
        Email = email;
    }

    protected Company() { }

    public string Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    
    public static Company Create(
        string name,
        string emailAddress)
    {
        Guard.IsNotNullOrWhiteSpace(name, nameof(name));
        Guard.IsNotNullOrWhiteSpace(emailAddress, nameof(emailAddress));

        var email = Email.Create(emailAddress);
        var company = new Company(name, email);
        company.RaiseDomainEvent(new CompanyCreatedDomainEvent(
            company.Id,
            company.Name,
            company.Email.Address));

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
        RaiseDomainEvent(new CompanyUpdatedDomainEvent(
            Id,
            Name,
            Email.Address));
    }
}