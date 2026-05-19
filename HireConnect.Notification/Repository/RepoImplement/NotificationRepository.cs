using Microsoft.EntityFrameworkCore;

using HireConnect.Notification.Models;
using HireConnect.Notification.Repository.Interface;
using HireConnect.Notification.Database;

namespace HireConnect.Notification.Repository.Implementation;

public class NotificationRepository
    : INotificationRepository
{
    private readonly NotificationDbContext _context;

    public NotificationRepository(NotificationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(NotificationIdentity notification)
    {
        await _context.Notifications.AddAsync(notification);
    }

    public async Task<List<NotificationIdentity>> GetUserNotificationsAsync(int userId)
    {
        return await _context.Notifications
            .Where(n =>
                n.UserId == userId
            )
            .OrderByDescending(n =>
                n.CreatedAt
            )
            .ToListAsync();
    }

    public async Task<NotificationIdentity?>GetByIdAsync(int notificationId)
    {
        return await _context.Notifications
            .FirstOrDefaultAsync(n =>
                n.NotificationId ==
                notificationId
            );
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}