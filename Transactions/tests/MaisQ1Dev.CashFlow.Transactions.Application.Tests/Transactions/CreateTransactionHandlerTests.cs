namespace MaisQ1Dev.CashFlow.Transactions.Application.Tests.Transactions;

public class CreateTransactionHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
    private readonly CreateTransactionHandler _handler;

    public CreateTransactionHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _transactionRepositoryMock = new Mock<ITransactionRepository>();

        _handler = new CreateTransactionHandler(
            _unitOfWorkMock.Object,
            _companyRepositoryMock.Object,
            _transactionRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnResultCreated()
    {
        // Arrange
        var company = CompanyMother.Create("Shrek Inc", "ceo@shrekinc.com");
        _companyRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(company);

        var command = new CreateTransactionCommand(
            company.Id,
            ETransactionType.Income,
            DateTime.UtcNow,
            100m,
            "Description");

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

    [Fact]
    public async Task Handle_WhenCompanyDoesNotExist_ShouldReturnResultNotFound()
    {
        // Arrange
        _companyRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Company)null);

        var command = new CreateTransactionCommand(
            Guid.NewGuid(),
            ETransactionType.Income,
            DateTime.UtcNow,
            100m,
            "Description");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<Result<Guid>>();
            result.Code.Should().Be(404);
            result.Errors.Should().HaveCount(1);
            result.Errors.First().Should().Be(CompanyError.NotFound);
        }
    }
}
