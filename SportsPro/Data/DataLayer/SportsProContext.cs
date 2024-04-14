/*
 * Quinton Nelson
 * 04 / 14 / 2024
 * This class is the context class for the SportsPro database. It contains the DbSets for the database tables and the OnModelCreating method to configure the database.
 */


using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
using SportsPro.Data.Configuration;

namespace SportsPro.Data.DataLayer
{
    public class SportsProContext : DbContext
    {
        public SportsProContext(DbContextOptions<SportsProContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CountryConfig());
            modelBuilder.ApplyConfiguration(new CustomerConfig());
            modelBuilder.ApplyConfiguration(new IncidentConfig());
            modelBuilder.ApplyConfiguration(new ProductConfig());
            modelBuilder.ApplyConfiguration(new TechnicianConfiguration());
            modelBuilder.ApplyConfiguration(new RegistrationConfig());
        }
    }
}
