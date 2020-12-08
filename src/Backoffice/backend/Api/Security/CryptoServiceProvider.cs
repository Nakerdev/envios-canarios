namespace CanaryDeliveries.Backoffice.Api.Security
{
    public interface CryptoServiceProvider
    {
        bool Verify(string passwordIntent, string passwordHash);
    }
}