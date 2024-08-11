
namespace MaisQ1Dev.CashFlow.Transactions.Application.Tests.Companies.DomainEvents;

public class CompanyCreatedDomainEventHandlerTests
{
    [Fact]
    public async Task HandleAsync_WithValidNotification_ShouldPublishIntegrationEvent()
    {
        // Arrange
        var eventBusMock = new Mock<IEventBus>();
        var handler = new CompanyCreatedDomainEventHandler(eventBusMock.Object);

        var notification = new CompanyCreatedDomainEvent(
            Guid.NewGuid(),
            "PrimaryCompany",
            "primary@company.com");

        // Act
        await handler.Handle(notification, CancellationToken.None);

        // Assert
        eventBusMock.Verify(
            x => x.PublishAsync(
                It.Is<CompanyCreatedIntegrationEvent>(
                    e => e.CompanyId == notification.CompanyId
                         && e.Name == notification.Name
                         && e.Email == notification.Email)),
            Times.Once);
    }
}
