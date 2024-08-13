namespace MaisQ1Dev.CashFlow.Transactions.Application.Tests.Companies;

public class ListCompanyHandlerTests
{
    private readonly Mock<ICashFlowTransactionDbContext> _contextMock;
    private readonly Mock<DbSet<Company>> _companyDbSetMock;
    private readonly ListCompanyHandler _handler;

    public ListCompanyHandlerTests()
    {
        _contextMock = new Mock<ICashFlowTransactionDbContext>();
        _companyDbSetMock = MoqExtensions.DbSetMock<Company>([
            CompanyMother.RitaESaraEletronica,
            CompanyMother.OtavioELuciaConstrucoes
        ]);

        _contextMock
            .Setup(x => x.Companies)
            .Returns(_companyDbSetMock.Object);

        _handler = new ListCompanyHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCompanyExists_ShouldReturnResultOk()
    {
        // Arrange
        var query = new ListCompanyQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<Result<IEnumerable<CompanyResponse>>>()
                .Which.Value
                .Should().BeEquivalentTo(new CompanyResponse[]
                {
                    CompanyResponse.FromCompany(CompanyMother.RitaESaraEletronica),
                    CompanyResponse.FromCompany(CompanyMother.OtavioELuciaConstrucoes)
                });
            result.Code.Should().Be(200);
        }
    }

    [Fact]
    public async Task Handle_WhenThereAreNoCompanies_ShouldReturnResultOkAndEmptyList()
    {
        // Arrange
        var emptyCompanyDbSetMock = MoqExtensions.DbSetMock<Company>(new List<Company>());

        _contextMock
            .Setup(x => x.Companies)
            .Returns(emptyCompanyDbSetMock.Object);

        var query = new ListCompanyQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<Result<IEnumerable<CompanyResponse>>>();
            result.Code.Should().Be(200);
            result.Value.Should().BeEmpty();
        }
    }

}
