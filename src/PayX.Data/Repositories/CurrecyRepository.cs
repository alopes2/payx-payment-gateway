using PayX.Core.Models;
using PayX.Core.Repositories;

namespace PayX.Data.Repositories
{
    public class CurrecyRepository : Repository<Currency>, ICurrencyRepository
    {
        private readonly PayXDbContext _dbContext;

        public CurrecyRepository(PayXDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        } 
    }
}