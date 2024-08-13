namespace MaisQ1Dev.CashFlow.Transactions.Domain.Tests.Companies;

public class CompanyTests
{
    [Fact]
    public void Create_ShouldReturnCreateCompany()
    {
        // Arrange
        var name = "Company Name";
        var emailAddress = "email@domain.com";

        // Act
        var company = Company.Create(name, emailAddress);

        // Assert
        using (new AssertionScope())
        {
            company.Id.Should().NotBeEmpty();
            company.Name.Should().Be(name);
            company.Email.Address.Should().Be(emailAddress);
            company.DomainEvents.OfType<CompanyCreatedDomainEvent>().Should().HaveCount(1);
        }
    }

    [Fact]
    public void Create_WithEmptyName_ShouldReturnThrowException()
    {
        // Arrange
        var name = string.Empty;
        var emailAddress = "email@domain.com";

        // Act
        Action act = () => Company.Create(name, emailAddress);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("name");
    }

    [Fact]
    public void Create_WithEmptyEmailAddress_ShouldReturnThrowException()
    {
        // Arrange
        var name = "Company Name";
        var emailAddress = string.Empty;

        // Act
        Action act = () => Company.Create(name, emailAddress);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("emailAddress");
    }

    [Fact]
    public void Create_WithInvalidEmailAddress_ShouldReturnThrowException()
    {
        // Arrange
        var name = "Company Name";
        var emailAddress = "email@domain";

        // Act
        Action act = () => Company.Create(name, emailAddress);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_ShouldReturnUpdateCompany()
    {
        // Arrange
        var company = Company.Create("Company Name", "email@domain.com");
        var name = "New Company Name";
        var emailAddress = "contact@domain.com";

        // Act
        company.Update(name, emailAddress);

        // Assert
        using (new AssertionScope())
        {
            company.Name.Should().Be(name);
            company.Email.Address.Should().Be(emailAddress);
            company.DomainEvents.OfType<CompanyUpdatedDomainEvent>().Should().HaveCount(1);
        }
    }

    [Fact]
    public void Update_WithEmptyName_ShouldReturnThrowException()
    {
        // Arrange
        var company = Company.Create("Company Name", "email@domain.com");
        var name = string.Empty;
        var emailAddress = "email@domain.com";

        // Act
        Action act = () => company.Update(name, emailAddress);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("name");
    }

    [Fact]
    public void Update_WithEmptyEmailAddress_ShouldReturnThrowException()
    {
        // Arrange
        var company = Company.Create("Company Name", "email@domain.com");
        var name = "New Company Name";
        var emailAddress = string.Empty;

        // Act
        Action act = () => company.Update(name, emailAddress);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("emailAddress");
    }

    [Fact]
    public void Update_WithInvalidEmailAddress_ShouldReturnThrowException()
    {
        // Arrange
        var company = Company.Create("Company Name", "email@domain.com");
        var name = "New Company Name";
        var emailAddress = "email@domain";

        // Act
        Action act = () => company.Update(name, emailAddress);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
