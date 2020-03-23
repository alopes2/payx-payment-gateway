using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<Payment>> GetAllWithCurrencyByUserIdAsync(Guid userId)
        {
            return await _dbContext
                .Payments
                .Include(p => p.Currency)
                .Include(p => p.User)
                .Where(p => p.UserId.Equals(userId))
                .ToListAsync();
        }

        public async Task<Payment> GetWithCurrencyByIdAsync(Guid id)
        {
            return await _dbContext
                .Payments
                .Include(p => p.Currency)
                .Include(p => p.User)
                .SingleOrDefaultAsync(p => p.Id.Equals(id));
        }
    }
}