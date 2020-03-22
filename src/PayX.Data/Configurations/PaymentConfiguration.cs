using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using PayX.Core.Models;

namespace PayX.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Id)
                .IsRequired();

            builder
                .Property(p => p.CardNumber)
                .IsRequired()
                .HasMaxLength(22);

            builder
                .Property(p => p.ExpirationMonth)
                .IsRequired();

            builder
                .Property(p => p.ExpirationYear)
                .IsRequired();

            builder
                .Property(p => p.Amount)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            builder
                .Property(p => p.CurrencyId)
                .IsRequired();

            builder
                .Property(p => p.Cvv)
                .HasMaxLength(999)
                .IsRequired();

            builder
                .Property(p => p.CreatedAt)
                .IsRequired();
        }
    }
}