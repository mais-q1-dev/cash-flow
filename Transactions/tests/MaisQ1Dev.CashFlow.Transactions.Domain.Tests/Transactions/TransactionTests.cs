using MaisQ1Dev.CashFlow.Transactions.Domain.Transactions;

namespace MaisQ1Dev.CashFlow.Transactions.Domain.Tests.Transactions;

public class TransactionTests
{
    [Fact]
    public void Create_ShouldCreateTransaction()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var type = ETransactionType.Income;
        var date = DateTime.UtcNow;
        var amount = 100m;
        var description = "Transaction description";

        // Act
        var transaction = Transaction.Create(companyId, type, date, amount, description);

        // Assert
        using (new AssertionScope())
        {
            transaction.CompanyId.Should().Be(companyId);
            transaction.Type.Should().Be(type);
            transaction.Date.Should().Be(date);
            transaction.Amount.Should().Be(amount);
            transaction.Description.Should().Be(description);
        }
    }

    [Fact]
    public void Create_WithTypeIsExpense_ShouldCreateTransactionWithAmountNegative()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var type = ETransactionType.Expense;
        var date = DateTime.UtcNow;
        var amount = 100m;
        var description = "Transaction description";

        // Act
        var transaction = Transaction.Create(companyId, type, date, amount, description);

        // Assert
        using (new AssertionScope())
        {
            transaction.CompanyId.Should().Be(companyId);
            transaction.Type.Should().Be(type);
            transaction.Date.Should().Be(date);
            transaction.Amount.Should().Be(-amount);
            transaction.Description.Should().Be(description);
        }
    }

    [Fact]
    public void Create_WithCompanyIsNull_ShouldReturnThrowException()
    {
        // Arrange
        var companyId = Guid.Empty;
        var type = ETransactionType.Income;
        var date = DateTime.UtcNow;
        var amount = 100m;
        var description = "Transaction description";

        // Act
        Action act = () => Transaction.Create(companyId, type, date, amount, description);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("companyId");
    }

    [Fact]
    public void Create_WithDateIsNull_ShouldReturnThrowException()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var type = ETransactionType.Income;
        var date = DateTime.MinValue;
        var amount = 100m;
        var description = "Transaction description";

        // Act
        Action act = () => Transaction.Create(companyId, type, date, amount, description);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("date");
    }

    [Fact]
    public void Create_WithAmountIsZero_ShouldReturnThrowException()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var type = ETransactionType.Income;
        var date = DateTime.UtcNow;
        var amount = 0m;
        var description = "Transaction description";

        // Act
        Action act = () => Transaction.Create(companyId, type, date, amount, description);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("amount");
    }

    [Fact]
    public void Create_WithAmountIsNegative_ShouldReturnThrowException()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var type = ETransactionType.Income;
        var date = DateTime.UtcNow;
        var amount = -100m;
        var description = "Transaction description";

        // Act
        Action act = () => Transaction.Create(companyId, type, date, amount, description);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("amount");
    }

    [Fact]
    public void Sync_ShouldSyncTransaction()
    {
        // Arrange
        var transaction = Transaction.Create(
            Guid.NewGuid(),
            ETransactionType.Income,
            DateTime.UtcNow,
            100m,
            "Transaction description");

        // Act
        transaction.Sync();

        // Assert
        transaction.SyncStatus.Should().Be(ETransactionSyncStatus.Synced);
    }

    [Fact]
    public void Update_ShouldUpdateTransaction()
    {
        // Arrange
        var transaction = Transaction.Create(
            Guid.NewGuid(),
            ETransactionType.Income,
            DateTime.UtcNow,
            100m,
            "Transaction description");

        var type = ETransactionType.Income;
        var date = DateTime.UtcNow.AddDays(1);
        var amount = 200m;
        var description = "Transaction description updated";

        // Act
        transaction.Update(type, date, amount, description);

        // Assert
        using (new AssertionScope())
        {
            transaction.CompanyId.Should().Be(transaction.CompanyId);
            transaction.Type.Should().Be(type);
            transaction.Date.Should().Be(date);
            transaction.Amount.Should().Be(amount);
            transaction.Description.Should().Be(description);
        }
    }

    [Fact]
    public void Update_WithTypeIsExpense_ShouldUpdateTransactionWithAmountNegative()
    {
        // Arrange
        var transaction = Transaction.Create(
            Guid.NewGuid(),
            ETransactionType.Income,
            DateTime.UtcNow,
            100m,
            "Transaction description");

        var type = ETransactionType.Expense;
        var date = DateTime.UtcNow.AddDays(1);
        var amount = 200m;
        var description = "Transaction description updated";

        // Act
        transaction.Update(type, date, amount, description);

        // Assert
        using (new AssertionScope())
        {
            transaction.CompanyId.Should().Be(transaction.CompanyId);
            transaction.Type.Should().Be(type);
            transaction.Date.Should().Be(date);
            transaction.Amount.Should().Be(-amount);
            transaction.Description.Should().Be(description);
        }
    }

    [Fact]
    public void Update_WithDateIsNull_ShouldReturnThrowException()
    {
        // Arrange
        var transaction = Transaction.Create(
            Guid.NewGuid(),
            ETransactionType.Income,
            DateTime.UtcNow,
            100m,
            "Transaction description");

        var type = ETransactionType.Expense;
        var date = DateTime.MinValue;
        var amount = 200m;
        var description = "Transaction description updated";

        // Act
        Action act = () => transaction.Update(type, date, amount, description);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("date");
    }

    [Fact]
    public void Update_WithAmountIsZero_ShouldReturnThrowException()
    {
        // Arrange
        var transaction = Transaction.Create(
            Guid.NewGuid(),
            ETransactionType.Income,
            DateTime.UtcNow,
            100m,
            "Transaction description");

        var type = ETransactionType.Expense;
        var date = DateTime.UtcNow.AddDays(1);
        var amount = 0m;
        var description = "Transaction description updated";

        // Act
        Action act = () => transaction.Update(type, date, amount, description);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("amount");
    }

    [Fact]
    public void Update_WithAmountIsNegative_ShouldReturnThrowException()
    {
        // Arrange
        var transaction = Transaction.Create(
            Guid.NewGuid(),
            ETransactionType.Income,
            DateTime.UtcNow,
            100m,
            "Transaction description");

        var type = ETransactionType.Expense;
        var date = DateTime.UtcNow.AddDays(1);
        var amount = -200m;
        var description = "Transaction description updated";

        // Act
        Action act = () => transaction.Update(type, date, amount, description);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("amount");
    }
}