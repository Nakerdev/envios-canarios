using Microsoft.EntityFrameworkCore;

namespace CanaryDeliveries.PurchaseApplication.DbContext
{
    public sealed class PurchaseApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        //public DbSet<PurchaseApplication> PurchaseApplications { get; set; } 
        public DbSet<Client> Clients { get; set; } 
        //public DbSet<Product> Products { get; set; } 
        
        private readonly string connectionString;

        public PurchaseApplicationDbContext(string connectionString) : base()
        {
            this.connectionString = connectionString;
        }

        public PurchaseApplicationDbContext() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}