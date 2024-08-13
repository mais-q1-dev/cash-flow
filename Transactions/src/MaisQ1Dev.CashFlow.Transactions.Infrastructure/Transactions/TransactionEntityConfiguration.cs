using MaisQ1Dev.CashFlow.Transactions.Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaisQ1Dev.CashFlow.Transactions.Infrastructure.Transactions;

public class TransactionEntityConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");

        builder.HasKey(transaction => transaction.Id);

        builder.Property(transaction => transaction.Version)
            .IsRowVersion();

        builder.Property(transaction => transaction.CreatedAt)
            .IsRequired();

        builder.Property(transaction => transaction.UpdatedAt);

        builder.Property(transaction => transaction.CompanyId)
            .IsRequired();

        builder
            .HasOne(transaction => transaction.Company)
            .WithMany()
            .HasForeignKey(transaction => transaction.CompanyId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(transaction => transaction.Type)
            .IsRequired();

        builder.Property(transaction => transaction.Date)
            .IsRequired();

        builder.Property(transaction => transaction.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(transaction => transaction.Description)
            .HasMaxLength(250);

        builder.Property(transaction => transaction.SyncStatus)
            .IsRequired();

        builder.HasIndex(transaction => transaction.Date);
    }
}
