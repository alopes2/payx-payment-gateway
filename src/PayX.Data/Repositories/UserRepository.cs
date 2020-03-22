using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PayX.Core.Models;
using PayX.Core.Models.Auth;
using PayX.Core.Repositories;

namespace PayX.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly PayXDbContext _dbContext;

        public UserRepository(PayXDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetByEmail(string email)
        {
            var normalizedEmail = email.ToLower();

            return await _dbContext
                .Users
                .SingleOrDefaultAsync(u => u.Email.Equals(normalizedEmail));
        }
    }
}