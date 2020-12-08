namespace CanaryDeliveries.Backoffice.Api.Security
{
    public sealed class BCryptCryptoServiceProvider : CryptoServiceProvider
    {
        public bool Verify(string passwordIntent, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(passwordIntent, passwordHash);
        }
    }
}