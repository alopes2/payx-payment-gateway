using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PayX.Core.Models;
using PayX.Core.Models.Auth;

namespace PayX.Data
{
    public class PayXDbContext : DbContext
    {
        public DbSet<Payment> Payments { get; set; }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<User> Users { get; set; }

        public PayXDbContext(DbContextOptions<PayXDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var currentAssembly = Assembly.GetExecutingAssembly();

            builder
                .ApplyConfigurationsFromAssembly(currentAssembly);
        }
    }
}
