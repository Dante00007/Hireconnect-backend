using Microsoft.EntityFrameworkCore;
using HireConnect.Application.Models;

namespace HireConnect.Application.Database
{
    
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ApplicationIdentity> Applications { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<ApplicationIdentity>()
                .Property(a => a.Status)
                .HasDefaultValue("Applied");

           
            modelBuilder.Entity<ApplicationIdentity>()
                .HasIndex(a => a.JobId);
            
            modelBuilder.Entity<ApplicationIdentity>()
                .HasIndex(a => a.CandidateId);
        }
    }
}