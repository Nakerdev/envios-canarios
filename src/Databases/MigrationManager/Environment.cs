namespace CanaryDeliveries.Databases.MigrationManager
{
    public static class Environment
    {
        public static string PurchaseApplicationDbConnectionString => System.Environment.GetEnvironmentVariable("PurchaseApplicationDbConnectionString");
    }
}