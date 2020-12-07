using System.Text;

namespace CanaryDeliveries.Backoffice.Api.Configuration
{
    public static class Environment
    {
        public static string PurchaseApplicationDbConnectionString => System.Environment.GetEnvironmentVariable("PurchaseApplicationDbConnectionString");
        public static byte[] JsonWebTokenSecretKey => Encoding.ASCII.GetBytes(System.Environment.GetEnvironmentVariable("JsonWebTokenSecretKey"));
    }
}