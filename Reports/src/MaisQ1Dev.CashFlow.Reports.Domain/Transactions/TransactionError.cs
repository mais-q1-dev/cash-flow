using MaisQ1Dev.Libs.Domain;

namespace MaisQ1Dev.CashFlow.Reports.Domain.Transactions;

public static class TransactionError
{
    public static Error NotFound = new("Transaction.NotFound", "Transaction not found");
    public static Error AlreadyExists => new("Transaction.AlreadyExists", "Transaction id already exists");
}