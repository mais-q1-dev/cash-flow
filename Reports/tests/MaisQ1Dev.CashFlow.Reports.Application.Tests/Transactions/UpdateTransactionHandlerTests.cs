using FluentAssertions;
using FluentAssertions.Execution;
using MaisQ1Dev.CashFlow.Reports.Application.Tests.Utils;
using MaisQ1Dev.CashFlow.Reports.Application.Transactions.UpdateTransaction;
using MaisQ1Dev.CashFlow.Reports.Domain.Companies;
using MaisQ1Dev.CashFlow.Reports.Domain.Transactions;
using MaisQ1Dev.Libs.Domain;
using MaisQ1Dev.Libs.Domain.Database;
using MaisQ1Dev.Libs.IntegrationEvents.EventBus;
using MaisQ1Dev.Libs.IntegrationEvents.Transaction;
using Moq;

namespace MaisQ1Dev.CashFlow.Reports.Application.Tests.Transactions;

public class UpdateTransactionHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
    private readonly Mock<IEventBus> _eventBusMock;
    private readonly UpdateTransactionHandler _handler;

    public UpdateTransactionHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _eventBusMock = new Mock<IEventBus>();

        _handler = new UpdateTransactionHandler(
            _unitOfWorkMock.Object,
            _companyRepositoryMock.Object,
            _transactionRepositoryMock.Object,
            _eventBusMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnResultNoContent()
    {
        // Arrange
        var company = CompanyMother.Create(name: "Shrek Inc", email: "ceo@shrekinc.com");
        company.UpdateBalance(100m);
        _companyRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(company);

        Company? companyWithBalanceUpdated = null;
        _companyRepositoryMock
            .Setup(m => m.Update(It.IsAny<Company>()))
            .Callback<Company>(company => companyWithBalanceUpdated = company);

        var transaction = TransactionMother.Create(
            companyId: company.Id,
            date: DateTime.UtcNow,
            amount: 100m,
            description: "Description");
        _transactionRepositoryMock
            .Setup(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(transaction);
        
        var command = new UpdateTransactionCommand(
            transaction.Id,
            transaction.CompanyId,
            transaction.Date,
            200m,
            transaction.Description);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<Result>();
            result.Code.Should().Be(204);

            _transactionRepositoryMock.Verify(x => x.Update(It.IsAny<Transaction>()), Times.Once);
            _companyRepositoryMock.Verify(x => x.Update(It.IsAny<Company>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _eventBusMock.Verify(x => x.PublishAsync(It.IsAny<TransactionSyncIntegrationEvent>()), Times.Once);

            companyWithBalanceUpdated!.Balance.Should().Be(200m);
        }
    }
}
