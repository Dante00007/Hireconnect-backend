using Microsoft.EntityFrameworkCore;
using HireConnect.Profile.Models;

namespace HireConnect.Profile.Database
{
    public class ProfileDbContext : DbContext
    {
        public ProfileDbContext(DbContextOptions<ProfileDbContext> options) : base(options) { }

        public DbSet<CandidateProfile> Candidates { get; set; }
        public DbSet<RecruiterProfile> Recruiters { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<CandidateProfile>().HasIndex(c => c.UserId).IsUnique();
            modelBuilder.Entity<RecruiterProfile>().HasIndex(r => r.UserId).IsUnique();


            base.OnModelCreating(modelBuilder);
        }
    }
}