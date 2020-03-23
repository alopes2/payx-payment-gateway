using PayX.Core.Services;

namespace PayX.Service
{
    public class EncryptionService : IEncryptionService
    {
        public string Hash(string text)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(text, 12);
        }

        public bool Verify(string text, string hashedText)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(text, hashedText);
        }
    }
}