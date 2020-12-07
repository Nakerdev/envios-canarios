using Microsoft.EntityFrameworkCore;

namespace CanaryDeliveries.PurchaseApplication.DbContext
{
    public sealed class PurchaseApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<PurchaseApplication> PurchaseApplications { get; set; } 
        public DbSet<Client> Clients { get; set; } 
        public DbSet<Product> Products { get; set; } 
        
        private readonly string connectionString;

        public PurchaseApplicationDbContext(string connectionString) : base()
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
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