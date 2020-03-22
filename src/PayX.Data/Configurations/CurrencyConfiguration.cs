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

                SeedCurrencyData(builder);
            }

            private static void SeedCurrencyData(EntityTypeBuilder<Currency> builder)
            {
                var seedCurrencies = new Currency[]
                {
                    new Currency
                    {
                        Id = new Guid("c48ab9b3-1f23-461a-a993-c6c040eb7d6b"),
                        Name = "EUR",
                    },
                    new Currency
                    {
                        Id = new Guid("ab8ab9b3-1b23-46aa-aba3-c6c040eb7d6c"),
                        Name = "USD",
                    },
                    new Currency
                    {
                        Id = new Guid("c48ab9c3-af23-461a-a103-c6c040eb7aba"),
                        Name = "BRL",
                    },
                };

                builder
                    .HasData(seedCurrencies);
            }
        }
}