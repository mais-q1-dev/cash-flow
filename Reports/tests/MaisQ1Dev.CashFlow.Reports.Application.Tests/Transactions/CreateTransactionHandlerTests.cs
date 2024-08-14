using FluentAssertions;
using FluentAssertions.Execution;
using MaisQ1Dev.CashFlow.Reports.Application.Tests.Utils;
using MaisQ1Dev.CashFlow.Reports.Application.Transactions.CreateTransaction;
using MaisQ1Dev.CashFlow.Reports.Domain.Companies;
using MaisQ1Dev.CashFlow.Reports.Domain.Transactions;
using MaisQ1Dev.Libs.Domain;
using MaisQ1Dev.Libs.Domain.Database;
using MaisQ1Dev.Libs.Domain.Logging;
using MaisQ1Dev.Libs.IntegrationEvents.EventBus;
using MaisQ1Dev.Libs.IntegrationEvents.Transaction;
using Moq;

namespace MaisQ1Dev.CashFlow.Reports.Application.Tests.Transactions;

public class CreateTransactionHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
    private readonly Mock<IEventBus> _eventBusMock;
    private readonly Mock<ILoggerMQ1Dev<CreateTransactionHandler>> _loggerMock;
    private readonly CreateTransactionHandler _handler;

    public CreateTransactionHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _eventBusMock = new Mock<IEventBus>();
        _loggerMock = new Mock<ILoggerMQ1Dev<CreateTransactionHandler>>();

        _handler = new CreateTransactionHandler(
            _unitOfWorkMock.Object,
            _companyRepositoryMock.Object,
            _transactionRepositoryMock.Object,
            _eventBusMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnResultCreated()
    {
        // Arrange
        var company = CompanyMother.Create(name: "Shrek Inc", email: "ceo@shrekinc.com");
        _companyRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(company);

        Company? companyWithBalanceUpdated = null;
        _companyRepositoryMock
            .Setup(m => m.Update(It.IsAny<Company>()))
            .Callback<Company>(company => companyWithBalanceUpdated = company);

        var transactionId = Guid.NewGuid();
        var command = new CreateTransactionCommand(
            transactionId,
            company.Id,
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
            result.Value.Should().Be(transactionId);

            _transactionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), Times.Once);
            _companyRepositoryMock.Verify(x => x.Update(It.IsAny<Company>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _eventBusMock.Verify(x => x.PublishAsync(It.IsAny<TransactionSyncIntegrationEvent>()), Times.Once);

            companyWithBalanceUpdated!.Balance.Should().Be(100m);
        }
    }
}
