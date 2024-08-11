namespace MaisQ1Dev.CashFlow.Transactions.Application.Tests.Companies;

public class GetCompanyByIdHandlerTests
{
    private readonly Company _companyPrimary = Company.Create("Primary Company", "primary@company.com");
    private readonly Company _companySecond = Company.Create("Second Company", "second@company.com");

    private readonly Mock<ICashFlowTransactionDbContext> _contextMock;
    private readonly Mock<DbSet<Company>> _companyDbSetMock;
    private readonly GetCompanyByIdHandler _handler;

    public GetCompanyByIdHandlerTests()
    {
        _contextMock = new Mock<ICashFlowTransactionDbContext>();
        _companyDbSetMock = MoqExtensions.DbSetMock<Company>([ _companyPrimary, _companySecond ]);

        _contextMock
            .Setup(x => x.Companies)
            .Returns(_companyDbSetMock.Object);

        _handler = new GetCompanyByIdHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCompanyExists_ShouldReturnResultOk()
    {
        // Arrange
        var query = new GetCompanyByIdQuery(_companyPrimary.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<Result<CompanyResponse>>();
            result.Code.Should().Be(200);
            result.Value.Should().BeEquivalentTo(CompanyResponse.FromCompany(_companyPrimary));
        }
    }

    [Fact]
    public async Task Handle_WhenCompanyDoesNotExist_ShouldReturnResultNotFound()
    {
        // Arrange
        var query = new GetCompanyByIdQuery(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<Result<CompanyResponse>>();
            result.Code.Should().Be(404);
            result.Errors.Should().HaveCount(1);
            result.Errors.First().Should().Be(CompanyError.NotFound);
        }
    }
}
