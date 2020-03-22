using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayX.Core.Models.Auth;

namespace PayX.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(c => c.Id);

            builder
                .Property(c => c.Id)
                .IsRequired()
                .HasValueGenerator(typeof(GuidValueGenerator));

            builder
                .Property(c => c.Email)
                .IsRequired();
                
            builder
                .Property(c => c.Password)
                .IsRequired();

            builder
                .Property(c => c.Role)
                .IsRequired()
                .HasDefaultValue(Role.Merchant);

            builder
                .HasIndex(c => c.Email)
                .IsUnique();
        }
    }
}