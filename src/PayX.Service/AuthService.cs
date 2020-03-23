using System;
using System.Threading.Tasks;
using PayX.Core;
using PayX.Core.Exceptions;
using PayX.Core.Models.Auth;
using PayX.Core.Services;

namespace PayX.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> SignInAsync(string email, string password)
        {
            var user = await _unitOfWork.Users.GetByEmail(email);

            var isValid = (user != null && BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password));
            if (isValid)
            {
                return user;
            }

            throw new HttpResponseException("User or password invalid.")
                .BadRequest("Invalid credentials");
        }

        public async Task<User> SignUpAsync(string email, string password)
        {
            var user = new User
            {
                Email = email,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 12)
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CommitAsync();

            return user;
        }
    }
}