using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Pharmacies.Data
{
    public class PharmacyContext : DbContext
    {
        public PharmacyContext(DbContextOptions<PharmacyContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Pharmacy>? Pharmacies { get; set; }
        public DbSet<OperationLog>? OperationLog { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pharmacy>().HasKey(x => x.ID);
            modelBuilder.Entity<Pharmacy>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<OperationLog>().HasKey(x => x.ID);
            
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
