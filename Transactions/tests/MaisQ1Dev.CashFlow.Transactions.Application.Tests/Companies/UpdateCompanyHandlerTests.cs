

using MaisQ1Dev.Libs.Domain;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Tests.Companies;

public class UpdateCompanyHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly UpdateCompanyHandler _handler;

    public UpdateCompanyHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _companyRepositoryMock = new Mock<ICompanyRepository>();

        _handler = new UpdateCompanyHandler(
            _unitOfWorkMock.Object,
            _companyRepositoryMock.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenCompanyExists_ShouldUpdateCompany()
    {
        // Arrange
        var company = Company.Create("Primary Company", "primary@company.com");
        _companyRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(company);

        var request = new UpdateCompanyCommand(
            company.Id,
            "New Company Name",
            "primary@company.com");

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Code.Should().Be(204);

            _companyRepositoryMock.Verify(
                x => x.UpdateAsync(
                    It.IsAny<Company>(), 
                    It.IsAny<CancellationToken>()), 
                Times.Once);
            
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    [Fact]
    public async Task HandleAsync_WhenCompanyDoesNotExist_ShouldReturnResultNotFound()
    {
        // Arrange
        _companyRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Company)null);

        var request = new UpdateCompanyCommand(
            Guid.NewGuid(),
            "New Company Name",
            "primary@company.com");

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Code.Should().Be(404);
            result.Errors.Should().HaveCount(1);
            result.Errors.First().Should().Be(CompanyError.NotFound);

            _companyRepositoryMock.Verify(
                x => x.UpdateAsync(
                    It.IsAny<Company>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
