using Microsoft.EntityFrameworkCore;
using PatientManagementSystem;
using PatientManagementSystem.Core.Models;

namespace PatientManagementSystem.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

    }
}