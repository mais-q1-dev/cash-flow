using MaisQ1Dev.CashFlow.Transactions.Application.Transactions.DomainEvents;
using MaisQ1Dev.CashFlow.Transactions.Domain.Transactions.DomainEvents;
using MaisQ1Dev.Libs.Domain.Logging;
using MaisQ1Dev.Libs.IntegrationEvents.Transaction;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Tests.Transactions.DomainEvents;

public class TransactionUpdatedDomainEventHandlerTests
{
    [Fact]
    public async Task Handle_WithValidNotification_ShouldPublishIntegrationEvent()
    {
        // Arrange
        var eventBusMock = new Mock<IEventBus>();
        var loggerMock = new Mock<ILoggerMQ1Dev<TransactionUpdatedDomainEventHandler>>();
        var handler = new TransactionUpdatedDomainEventHandler(eventBusMock.Object, loggerMock.Object);

        var notification = new TransactionUpdatedDomainEvent(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.Now,
            100.00m,
            "Transaction Updated");

        // Act
        await handler.Handle(notification, CancellationToken.None);

        // Assert
        eventBusMock.Verify(
            x => x.PublishAsync(
                It.Is<TransactionUpdatedIntegrationEvent>(
                    e => e.TransactionId == notification.TransactionId
                         && e.CompanyId == notification.CompanyId
                         && e.Date == notification.Date
                         && e.Amount == notification.Amount
                         && e.Description == notification.Description)),
            Times.Once);
    }
}
