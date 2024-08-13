namespace MaisQ1Dev.CashFlow.Transactions.Application.Tests.Transactions;

public class ListTransactionHandlerTests
{
    private readonly Mock<ICashFlowTransactionDbContext> _contextMock;
    private readonly Mock<DbSet<Transaction>> _transactionDbSetMock;
    private readonly ListTransactionHandler _handler;

    public ListTransactionHandlerTests()
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

        _handler = new ListTransactionHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_WhenTransactionExists_ShouldReturnResultOk()
    {
        // Arrange
        var companyId = CompanyMother.RitaESaraEletronica.Id;
        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);
        var pageNumber = 1;
        int pageSize = 10;

        var query = new ListTransactionQuery(companyId, startDate, endDate, pageNumber, pageSize);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<PagedResult<IEnumerable<TransactionResponse>>>();
            result.Should().BeEquivalentTo(new PagedResult<IEnumerable<TransactionResponse>>
                (
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    totalRecords: 2,
                    item:
                        [
                            TransactionResponse.FromTransaction(TransactionMother.Salary),
                            TransactionResponse.FromTransaction(TransactionMother.InternetSubscription)
                        ]
                ));
        }
    }

    [Fact]
    public async Task Handle_WhenThereAreNoTransactionsForTheCompanyid_ShouldReturnResultOkAndEmptyItems()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);
        var pageNumber = 1;
        int pageSize = 10;

        var query = new ListTransactionQuery(companyId, startDate, endDate, pageNumber, pageSize);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<PagedResult<IEnumerable<TransactionResponse>>>();
            result.Should().BeEquivalentTo(new PagedResult<IEnumerable<TransactionResponse>>
                (
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    totalRecords: 0,
                    item: []
                ));
        }
    }

    [Fact]
    public async Task Handle_WhenThereAreNoTransactionsForThePeriod_ShouldReturnResultOkAndEmptyItems()
    {
        // Arrange
        var companyId = CompanyMother.RitaESaraEletronica.Id;
        var startDate = DateTime.Today.AddDays(1);
        var endDate = DateTime.Today.AddDays(1).AddHours(23).AddMinutes(59).AddSeconds(59);
        var pageNumber = 1;
        int pageSize = 10;

        var query = new ListTransactionQuery(companyId, startDate, endDate, pageNumber, pageSize);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<PagedResult<IEnumerable<TransactionResponse>>>();
            result.Should().BeEquivalentTo(new PagedResult<IEnumerable<TransactionResponse>>
                (
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    totalRecords: 0,
                    item: []
                ));
        }
    }

    [Fact]
    public async Task Handle_WhenThereAreMoreTransactionsThanPageSize_ShouldReturnResultOkAndItemsWithPageSize()
    {
        // Arrange
        var companyId = CompanyMother.RitaESaraEletronica.Id;
        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);
        var pageNumber = 1;
        int pageSize = 1;

        var query = new ListTransactionQuery(companyId, startDate, endDate, pageNumber, pageSize);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<PagedResult<IEnumerable<TransactionResponse>>>();
            result.Should().BeEquivalentTo(new PagedResult<IEnumerable<TransactionResponse>>
                (
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    totalRecords: 2,
                    item: [TransactionResponse.FromTransaction(TransactionMother.Salary)]
                ));
        }
    }
}
