using PayX.Core.Models;
using PayX.Core.Repositories;

namespace PayX.Data.Repositories
{
    public class CurrencyRepository : Repository<Currency>, ICurrencyRepository
    {
        private readonly PayXDbContext _dbContext;

        public CurrencyRepository(PayXDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        } 
    }
}