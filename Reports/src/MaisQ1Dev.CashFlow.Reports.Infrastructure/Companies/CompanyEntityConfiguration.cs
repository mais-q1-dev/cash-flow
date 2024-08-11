using MaisQ1Dev.CashFlow.Reports.Domain.Companies;
using MaisQ1Dev.Libs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaisQ1Dev.CashFlow.Reports.Infrastructure.Companies;

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

        builder.Property(company => company.Balance)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
    }
}