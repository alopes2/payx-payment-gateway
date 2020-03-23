using System.Net;
using System.Threading.Tasks;
using Moq;
using PayX.Core;
using PayX.Core.Exceptions;
using PayX.Core.Models.Auth;
using PayX.Core.Services;
using PayX.Service;
using Xunit;

namespace PayX.UnitTests.Services
{
    public class AuthServiceTests
    {
        private AuthService _authService;

        private Mock<IUnitOfWork> _unitOfWork;

        private Mock<IEncryptionService> _encryptionService;
        
        public AuthServiceTests()
        {
            _encryptionService = new Mock<IEncryptionService>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _authService = new AuthService(_unitOfWork.Object, _encryptionService.Object);
        }

        [Fact]
        public async Task SignUp_ValidNonExistingUser_ReturnUserAsync()
        {
            var email = "test@test.com";
            var password = "123";
            var hashedPassword = "1234";

            User nonExistingUser = null;
            _unitOfWork.Setup(u => u.Users.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(nonExistingUser);

            _encryptionService
                .Setup(e => e.Hash(password))
                .Returns(hashedPassword);

            var result = await _authService.SignUpAsync(email, password);

            Assert.Equal(email, result.Email);
            Assert.Equal(hashedPassword, result.Password);

            ResetMocks();
        }

        [Fact]
        public async Task SignUp_EmailWithUppercaseLetters_ReturnUserEmailInLowerCaseAsync()
        {
            var email = "Test@TesT.com";
            var password = "123";

            User nonExistingUser = null;
            _unitOfWork.Setup(u => u.Users.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(nonExistingUser);

            var result = await _authService.SignUpAsync(email, password);

            Assert.Equal(email.ToLower(), result.Email);

            ResetMocks();
        }

        [Fact]
        public async Task SignUp_ExistingUser_ThrowsHttpResponseExceptionWithStatusBadRequest()
        {
            var email = "Test@TesT.com";
            var password = "123";

            var existingUser = new User();
            _unitOfWork.Setup(u => u.Users.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(existingUser);

            var exception = await Assert.ThrowsAsync<HttpResponseException>(() => _authService.SignUpAsync(email, password));

            Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);

            ResetMocks();
        }

        [Fact]
        public async Task SignIn_ExistingUserAndCorrectPassword_ReturnUser()
        {
            var email = "test@test.com";
            var password = "123";

            var existingUser = new User()
            {
                Email = email,
                Password = password
            };

            _unitOfWork.Setup(u => u.Users.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(existingUser);

            _encryptionService.Setup(e => e.Verify(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
                
            var result = await _authService.SignInAsync(email, password);

            Assert.Equal(email, result.Email);

            ResetMocks();
        }

        [Fact]
        public async Task SignIn_InvalidEmail_ThrowHttpResponseExceptionWithStatusBadRequest()
        {
            var email = "Test@Test.com";
            var password = "123";

            User nonExistingUser = null;

            _unitOfWork.Setup(u => u.Users.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(nonExistingUser);
                
            var exception = await Assert.ThrowsAsync<HttpResponseException>(() => _authService.SignInAsync(email, password));

            Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);

            ResetMocks();
        }

        [Fact]
        public async Task SignIn_ExistingUserButWrongPassword_ThrowHttpResponseExceptionWithStatusBadRequest()
        {
            var email = "Test@Test.com";
            var password = "123";

            User existingUser = new User();

            _unitOfWork.Setup(u => u.Users.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(existingUser);
                
            var exception = await Assert.ThrowsAsync<HttpResponseException>(() => _authService.SignInAsync(email, password));

            Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);

            ResetMocks();
        }

        private void ResetMocks()
        {
            _unitOfWork.Reset();
            _encryptionService.Reset();
        }
    }
}