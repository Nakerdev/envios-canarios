using System.Text;

namespace CanaryDeliveries.Backoffice.Api.Configuration
{
    public static class Environment
    {
        public static byte[] JsonWebTokenSecretKey => Encoding.UTF8.GetBytes(System.Environment.GetEnvironmentVariable("JsonWebTokenSecretKey"));
    }
}