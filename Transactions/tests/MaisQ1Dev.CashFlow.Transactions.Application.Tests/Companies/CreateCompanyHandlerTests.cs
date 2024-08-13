namespace MaisQ1Dev.CashFlow.Transactions.Application.Tests.Companies;

public class CreateCompanyHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly CreateCompanyHandler _handler;

    public CreateCompanyHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _companyRepositoryMock = new Mock<ICompanyRepository>();

        _handler = new CreateCompanyHandler(
            _unitOfWorkMock.Object,
            _companyRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnResultCreated()
    {
        // Arrange
        var command = new CreateCompanyCommand(
            "Company Name",
            "email@domain.com");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<Result<Guid>>();
            result.Code.Should().Be(201);
            result.Value.Should().NotBeEmpty();
        }
    }
}
