using Microsoft.EntityFrameworkCore;
using PatientManagementSystem.Core.Models;
using PatientManagementSystem.Data;
using PatientManagementSystem;
using System.Collections.Generic;

namespace PatientManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

    }
}