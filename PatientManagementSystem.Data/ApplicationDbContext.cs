using Microsoft.EntityFrameworkCore;
using PatientManagementSystem;
using System.Collections.Generic;

namespace PatientManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Define DbSet for the Patient entity
        public DbSet<Patient> Patients { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Add other DbSet properties for your entities as needed

        // Override OnModelCreating method if you need to configure entity relationships or apply additional configurations
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     // Configure entity relationships and other configurations
        // }
    }
}