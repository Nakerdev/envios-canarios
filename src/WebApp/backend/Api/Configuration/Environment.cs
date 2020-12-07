namespace CanaryDeliveries.WebApp.Api.Configuration
{
    public static class Environment
    {
        public static string PurchaseApplicationDbConnectionString => System.Environment.GetEnvironmentVariable("PurchaseApplicationDbConnectionString");
    }
}