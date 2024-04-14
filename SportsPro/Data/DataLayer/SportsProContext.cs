using Microsoft.EntityFrameworkCore;
using SportsPro.Data.Configuration;

namespace SportsPro.Data.DataLayer
{
    public class SportsProContext : DbContext
    {
        public SportsProContext(DbContextOptions<SportsProContext> options) : base(options) { }

        public DbSet<Models.Customer> Customers { get; set; }
        public DbSet<Models.Incident> Incidents { get; set; }
        public DbSet<Models.Product> Products { get; set; }
        public DbSet<Models.Registration> Registrations { get; set; }
        public DbSet<Models.Technician> Technicians { get; set; }
        public DbSet<Models.Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CountryConfig());
            modelBuilder.ApplyConfiguration(new CustomerConfig());
            modelBuilder.ApplyConfiguration(new IncidentConfig());
            modelBuilder.ApplyConfiguration(new ProductConfig());
            modelBuilder.ApplyConfiguration(new TechnicianConfiguration());
        }
    }
}
