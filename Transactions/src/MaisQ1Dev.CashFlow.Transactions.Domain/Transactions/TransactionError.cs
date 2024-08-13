using MaisQ1Dev.Libs.Domain;

namespace MaisQ1Dev.CashFlow.Transactions.Domain.Transactions;

public static class TransactionError
{
    public static Error NotFound = new("Transaction.NotFound", "Transaction not found");
}