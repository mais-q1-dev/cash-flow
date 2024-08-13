using FluentAssertions;
using MaisQ1Dev.CashFlow.Reports.Domain.Companies;

namespace MaisQ1Dev.CashFlow.Reports.Domain.Tests.Companies;

public class CompanyTests
{
    private readonly Company _shrekIncCompany;

    public CompanyTests()
    {
        _shrekIncCompany = Company.Create(
            Guid.NewGuid(),
            "Shrek Inc",
            "ceo@shrekinc.com");
    }

    [Fact]
    public void UpdateBalance_ShouldUpdatedBalance()
    {
        // Arrange
        var amount = 100m;

        // Act
        _shrekIncCompany.UpdateBalance(amount);

        // Assert
        _shrekIncCompany.Balance.Should().Be(amount);
    }

    [Fact]
    public void UpdateBalance_WhenAmountIsNegative_ShouldUpdateBalance()
    {
        // Arrange
        var amount = -100m;

        // Act
        _shrekIncCompany.UpdateBalance(amount);

        // Assert
        _shrekIncCompany.Balance.Should().Be(amount);
    }

    [Fact]
    public void UpdateBalance_WhenAlreadyBalancePositiveAndAmountIsNegative_ShouldUpdateBalance()
    {
        // Arrange
        _shrekIncCompany.UpdateBalance(100m);
        var amount = -200m;
        var expectedBalance = -100m;

        // Act
        _shrekIncCompany.UpdateBalance(amount);

        // Assert
        _shrekIncCompany.Balance.Should().Be(expectedBalance);
    }

    [Fact]
    public void UpdateBalance_WhenAlreadyBalanceNegativeAndAmountIsPositive_ShouldUpdateBalance()
    {
        // Arrange
        _shrekIncCompany.UpdateBalance(-100m);
        var amount = 150m;
        var expectedBalance = 50m;

        // Act
        _shrekIncCompany.UpdateBalance(amount);

        // Assert
        _shrekIncCompany.Balance.Should().Be(expectedBalance);
    }

    [Fact]
    public void UpdateBalance_WhenAlreadyBalanceNegativeAndAmountIsNegative_ShouldUpdateBalance()
    {
        // Arrange
        _shrekIncCompany.UpdateBalance(-100m);
        var amount = -150m;
        var expectedBalance = -250m;

        // Act
        _shrekIncCompany.UpdateBalance(amount);

        // Assert
        _shrekIncCompany.Balance.Should().Be(expectedBalance);
    }
}
