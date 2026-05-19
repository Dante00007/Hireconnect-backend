using Microsoft.EntityFrameworkCore;
using HireConnect.Notification.Models;

namespace HireConnect.Notification.Database;

public class NotificationDbContext : DbContext
{
    public DbSet<NotificationIdentity> Notifications { get; set; }
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
         : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<NotificationIdentity>().ToTable("Notifications");
    }
}