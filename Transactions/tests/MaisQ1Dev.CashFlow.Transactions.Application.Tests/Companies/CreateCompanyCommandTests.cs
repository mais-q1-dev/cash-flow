namespace MaisQ1Dev.CashFlow.Transactions.Application.Tests.Companies;

public class CreateCompanyCommandTests
{
    [Theory]
    [MemberData(nameof(SimulationCreateCompanyCommand))]
    public void Command_ShouldReturnErrorMessage_WhenDataIsInvalid(
       CreateCompanyCommand command,
       string expectedErrorMessage
   )
    {
        //Arrange && Act
        var errorMessage = new CreateCompanyCommandValidator().Validate(command);

        //Assert
        errorMessage.IsValid.Should().BeFalse();
        errorMessage.Errors.Count.Should().Be(1);
        errorMessage.Errors.First().ErrorMessage.Should().Be(expectedErrorMessage);
    }

    public static IEnumerable<object[]> SimulationCreateCompanyCommand()
    {
        yield return new object[]
        {
            new CreateCompanyCommand(
                Name: string.Empty,
                Email: "primary@company.com"
            ),
            "Name can't be empty"
        };

        yield return new object[]
        {
            new CreateCompanyCommand(
                Name: new string('a', 201),
                Email: "primary@company.com"
            ),
            "Name must be max 200 characters"
        };

        yield return new object[]
        {
            new CreateCompanyCommand(
                Name: "Primary Company",
                Email: string.Empty
            ),
            "E-mail can't be empty"
        };

        yield return new object[]
        {
            new CreateCompanyCommand(
                Name: "Primary Company",
                Email: "primary@company."
            ),
            "E-mail invalid"
        };

        yield return new object[]
        {
            new CreateCompanyCommand(
                Name: "Primary Company",
                Email: "@company.com"
            ),
            "E-mail invalid"
        };
    }
}
