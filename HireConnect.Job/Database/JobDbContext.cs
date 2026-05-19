using Microsoft.EntityFrameworkCore;
using HireConnect.Job.Models;

namespace HireConnect.Job.Database
{
    public class JobDbContext : DbContext
    {
        public JobDbContext(DbContextOptions<JobDbContext> options) : base(options) { }

        public DbSet<Jobs> Jobs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Jobs>()
                .Property(j => j.Skills)
                .HasColumnType("text[]");
        }
    }
}