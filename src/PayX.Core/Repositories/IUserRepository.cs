using System.Threading.Tasks;
using PayX.Core.Models.Auth;

namespace PayX.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    { 
        Task<User> GetByEmailAsync(string email);
    }
}