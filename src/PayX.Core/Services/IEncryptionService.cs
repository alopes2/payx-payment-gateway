namespace PayX.Core.Services
{
    public interface IEncryptionService
    {
        string Hash(string text);

        bool Verify(string text, string hashedText);
    }
}