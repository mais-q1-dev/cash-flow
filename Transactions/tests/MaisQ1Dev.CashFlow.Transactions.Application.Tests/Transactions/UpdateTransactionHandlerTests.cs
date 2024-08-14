using MaisQ1Dev.Libs.Domain.Logging;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Tests.Transactions;

public class UpdateTransactionHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
    private readonly Mock<ILoggerMQ1Dev<UpdateTransactionHandler>> _loggerMock;
    private readonly UpdateTransactionHandler _handler;

    public UpdateTransactionHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _loggerMock = new Mock<ILoggerMQ1Dev<UpdateTransactionHandler>>();

        _handler = new UpdateTransactionHandler(
            _unitOfWorkMock.Object,
            _companyRepositoryMock.Object,
            _transactionRepositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenTransactionExists_ShouldUpdateTransaction()
    {
        // Arrange
        var transaction = TransactionMother.Create(
            type: ETransactionType.Income,
            date: DateTime.UtcNow,
            amount: 100m,
            description: "Description");

        _transactionRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(transaction);

        var request = new UpdateTransactionCommand(
            transaction.Id,
            transaction.CompanyId,
            ETransactionType.Income,
            DateTime.UtcNow,
            50m,
            "New Description");

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Code.Should().Be(204);

            _transactionRepositoryMock.Verify(x => x.Update(It.IsAny<Transaction>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    [Fact]
    public async Task HandleAsync_WhenTransactionNotExists_ShouldReturnNotFound()
    {
        // Arrange
        _transactionRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Transaction)null);

        var request = new UpdateTransactionCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            ETransactionType.Income,
            DateTime.UtcNow,
            50m,
            "New Description");

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Code.Should().Be(404);
            result.Errors.Should().HaveCount(1);
            result.Errors.First().Should().Be(TransactionError.NotFound);

            _transactionRepositoryMock.Verify(x => x.Update(It.IsAny<Transaction>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }

    [Fact]
    public async Task HandleAsync_WhenTransactionCompanyNotMatch_ShouldReturnNotFound()
    {
        // Arrange
        var transaction = TransactionMother.Create(
            type: ETransactionType.Income,
            date: DateTime.UtcNow,
            amount: 100m,
            description: "Description");

        _transactionRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(transaction);

        var request = new UpdateTransactionCommand(
            transaction.Id,
            Guid.NewGuid(),
            ETransactionType.Income,
            DateTime.UtcNow,
            50m,
            "New Description");

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Code.Should().Be(404);
            result.Errors.Should().HaveCount(1);
            result.Errors.First().Should().Be(CompanyError.NotFound);

            _transactionRepositoryMock.Verify(x => x.Update(It.IsAny<Transaction>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
