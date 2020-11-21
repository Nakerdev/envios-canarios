using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CanaryDeliveries.PurchaseApplication.DbContext
{
    public sealed class PurchaseApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        //public DbSet<PurchaseApplication> PurchaseApplications { get; set; } 
        public DbSet<Client> Clients { get; set; } 
        public DbSet<Product> Products { get; set; } 
        
        private readonly IConfigurationRoot configuration;

        public PurchaseApplicationDbContext() : base()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = configuration.GetConnectionString("CanaryDeliveriesDbConnectionString");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}