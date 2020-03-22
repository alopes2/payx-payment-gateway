using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using PayX.Core.Models;

namespace PayX.Data.Configurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder
                .HasKey(c => c.Id);

            builder
                .Property(c => c.Id)
                .IsRequired()
                .HasValueGenerator(typeof(GuidValueGenerator));

            builder
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(3);

            builder
                .HasIndex(c => c.Name)
                .IsUnique();
        }
    }
}