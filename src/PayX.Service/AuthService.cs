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
        private readonly IEncryptionService _encryptionService;

        public AuthService(IUnitOfWork unitOfWork, IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
            _unitOfWork = unitOfWork;
        }

        public async Task<User> SignInAsync(string email, string password)
        {
            var normalizedEmail = email.ToLower();
            var user = await _unitOfWork.Users.GetByEmail(normalizedEmail);

            var isValid = (user != null && _encryptionService.Verify(password, user.Password));
            if (isValid)
            {
                return user;
            }

            throw new HttpResponseException("User or password invalid.")
                .BadRequest("Invalid credentials");
        }

        public async Task<User> SignUpAsync(string email, string password)
        {
            var normalizedEmail = email.ToLower();
            var existingUser = _unitOfWork.Users.SingleOrDefaultAsync(u => u.Email.Equals(normalizedEmail));
            if (existingUser != null)
            {
                throw new HttpResponseException("User with this email already exists.")
                    .BadRequest("Email in use");
            }

            var user = new User
            {
                Email = normalizedEmail,
                Password = _encryptionService.Hash(password)
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CommitAsync();

            return user;
        }
    }
}