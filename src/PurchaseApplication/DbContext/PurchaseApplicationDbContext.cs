using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CanaryDeliveries.PurchaseApplication.DbContext
{
    public sealed class PurchaseApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<PurchaseApplication> PurchaseApplications { get; set; } 
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
            optionsBuilder.UseNpgsql(connectionString);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            const string DB_CONTEXT_PREFIX = "PurchaseApplication_";
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName($"{DB_CONTEXT_PREFIX }{entityType.GetTableName()}");
            }
        }
    }
}