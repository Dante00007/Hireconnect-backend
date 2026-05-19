using Microsoft.EntityFrameworkCore;
using HireConnect.Interview.Models;

namespace HireConnect.Interview.Data
{
    public class InterviewDbContext : DbContext
    {
        public InterviewDbContext(DbContextOptions<InterviewDbContext> options) : base(options) { }

        public DbSet<InterviewIdentity> Interviews { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InterviewIdentity>()
                .Property(i => i.Status)
                .HasDefaultValue("Scheduled");
        }
    }
}