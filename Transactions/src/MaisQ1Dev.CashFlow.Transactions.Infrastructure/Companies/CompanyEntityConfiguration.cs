using MaisQ1Dev.CashFlow.Transactions.Domain.Companies;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MaisQ1Dev.Libs.Domain.Entities;

namespace MaisQ1Dev.CashFlow.Transactions.Infrastructure.Companies;

public class CompanyEntityConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");

        builder.HasKey(company => company.Id);

        builder.Property(company => company.Version)
            .IsRowVersion();

        builder.Property(company => company.CreatedAt)
            .IsRequired();

        builder.Property(company => company.UpdatedAt);

        builder.Property(company => company.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(company => company.Email)
            .IsRequired()
            .HasMaxLength(255)
            .HasConversion(email => email.Address, address => Email.Create(address));
    }
}
