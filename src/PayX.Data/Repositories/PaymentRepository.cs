using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PayX.Core.Models;
using PayX.Core.Repositories;

namespace PayX.Data.Repositories
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        private readonly PayXDbContext _dbContext;
        public PaymentRepository(PayXDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Payment>> GetAllWithCurrentyAsync()
        {
            return await _dbContext
                .Payments
                .Include(p => p.Currency)
                .ToListAsync();
        }

        public async Task<Payment> GetWithCurrentyByIdAsync(Guid id)
        {
            return await _dbContext
                .Payments
                .Include(p => p.Currency)
                .SingleOrDefaultAsync(p => p.Id.Equals(id));
        }
    }
}