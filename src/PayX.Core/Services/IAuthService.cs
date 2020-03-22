using System.Threading.Tasks;
using PayX.Core.Models.Auth;

namespace PayX.Core.Services
{
    public interface IAuthService
    {
        Task<User> SignUpAsync(string email, string password);

        Task<User> SignInAsync(string email, string password);
    }
}