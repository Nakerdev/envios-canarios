using System;
using CanaryDeliveries.PurchaseApplication.DbContext;
using Microsoft.EntityFrameworkCore;

namespace CanaryDeliveries.Databases.MigrationManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Applying Purchase Applications DB Migrations..");
            using var dbContext = new PurchaseApplicationDbContext();
            dbContext.Database.Migrate();
            Console.WriteLine("Migrations applied.");
        }
    }
}