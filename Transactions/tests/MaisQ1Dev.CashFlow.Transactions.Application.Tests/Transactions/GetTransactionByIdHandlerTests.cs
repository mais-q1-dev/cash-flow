namespace MaisQ1Dev.CashFlow.Transactions.Application.Tests.Transactions;

public class GetTransactionByIdHandlerTests
{
    private readonly Mock<ICashFlowTransactionDbContext> _contextMock;
    private readonly Mock<DbSet<Transaction>> _transactionDbSetMock;
    private readonly GetTransactionByIdHandler _handler;

    public GetTransactionByIdHandlerTests()
    {
        _contextMock = new Mock<ICashFlowTransactionDbContext>();
        _transactionDbSetMock = MoqExtensions.DbSetMock<Transaction>([
            TransactionMother.Salary,
            TransactionMother.InternetSubscription,
            TransactionMother.InvestmentIncome
        ]);

        _contextMock
            .Setup(x => x.Transactions)
            .Returns(_transactionDbSetMock.Object);

        _handler = new GetTransactionByIdHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_WhenTransactionExists_ShouldReturnResultOk()
    {
        // Arrange
        var query = new GetTransactionByIdQuery(TransactionMother.Salary.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<Result<TransactionResponse>>();
            result.Code.Should().Be(200);
            result.Value.Should().BeEquivalentTo(TransactionResponse.FromTransaction(TransactionMother.Salary));
        }
    }

    [Fact]
    public async Task Handle_WhenTransactionDoesNotExist_ShouldReturnResultNotFound()
    {
        // Arrange
        var query = new GetTransactionByIdQuery(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<Result<TransactionResponse>>();
            result.Code.Should().Be(404);
            result.Errors.Should().HaveCount(1);
            result.Errors.First().Should().Be(TransactionError.NotFound);
        }
    }
}
