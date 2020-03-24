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

            builder
                .HasMany(u => u.Payments)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            SeedUsers(builder);
        }

        private static void SeedUsers(EntityTypeBuilder<User> builder)
        {
            var seedUsers = new User[]
            {
                new User
                {
                    Id = new Guid("5c6e15ff-508f-487a-a753-1119e831eabb"),
                    Email = "admin@payx.io",
                    Password = "$2a$12$iSfNL2fnxQN1hLVXd8PcT.5aorGzJFS8ARBZpDaEJkkQ8eniLP9X6",
                    Role = Role.Admin
                }
            };

            builder
                .HasData(seedUsers);
        }
    }
}