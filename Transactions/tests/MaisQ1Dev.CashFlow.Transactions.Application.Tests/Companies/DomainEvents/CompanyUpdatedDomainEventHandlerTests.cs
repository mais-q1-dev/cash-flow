using MaisQ1Dev.Libs.Domain.Logging;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Tests.Companies.DomainEvents;

public class CompanyUpdatedDomainEventHandlerTests
{
    [Fact]
    public async Task Handle_WithValidNotification_ShouldPublishEvent()
    {
        // Arrange
        var eventBusMock = new Mock<IEventBus>();
        var loggerMock = new Mock<ILoggerMQ1Dev<CompanyUpdatedDomainEventHandler>>();
        var handler = new CompanyUpdatedDomainEventHandler(eventBusMock.Object, loggerMock.Object);
        
        var notification = new CompanyUpdatedDomainEvent(
            Guid.NewGuid(),
            "Secondary Company",
            "secondary@company.com");

        // Act
        await handler.Handle(notification, CancellationToken.None);

        // Assert
        eventBusMock.Verify(
            x => x.PublishAsync(
                It.Is<CompanyUpdatedIntegrationEvent>(
                    e => e.CompanyId == notification.CompanyId
                         && e.Name == notification.Name
                         && e.Email == notification.Email)),
            Times.Once);
    }
}
