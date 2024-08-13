using FluentAssertions;
using MaisQ1Dev.Libs.Domain.Entities;

namespace MaisQ1Dev.Libs.Domain.Tests.Entities;

public class EmailTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_ShouldReturnUnprocessableEntity_WhenAddressIsEmpty(string? address)
    {
        // Arrange && Act
        Action act = () => Email.Create(address);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("address");
    }

    [Fact]
    public void Create_ShouldReturnUnprocessableEntity_WhenAddressHasLessThan6Characters()
    {
        // Arrange
        var address = "a@b.c";

        // Act
        Action act = () => Email.Create(address);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("address");
    }

    [Fact]
    public void Create_ShouldReturnUnprocessableEntity_WhenAddressHasMoreThan255Characters()
    {
        // Arrange
        var address = new string('a', 256) + "@domain.com";

        // Act
        Action act = () => Email.Create(address);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("EmailRegex().IsMatch(address)");
    }

    [Theory]
    [InlineData("email@")]
    [InlineData("email@domain")]
    [InlineData("email@domain.")]
    [InlineData("email@domain.c")]
    [InlineData("@domain.com")]
    [InlineData("email@.com")]
    [InlineData("invalid-email")]
    public void Create_ShouldReturnUnprocessableEntity_WhenAddressInvalid(string address)
    {
        // Arrange && Act
        Action act = () => Email.Create(address);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("EmailRegex().IsMatch(address)");
    }

    [Fact]
    public void Create_ShouldCreateEmail()
    {
        // Arrange
        var address = "email@domain.com";

        // Act
        var email = Email.Create(address);

        // Assert
        email.Address.Should().Be(address);
    }
}
