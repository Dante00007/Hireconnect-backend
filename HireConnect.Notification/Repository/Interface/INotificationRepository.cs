using HireConnect.Notification.Models;

namespace HireConnect.Notification.Repository.Interface;

public interface INotificationRepository
{
    Task AddAsync(NotificationIdentity notification);

    Task<List<NotificationIdentity>>GetUserNotificationsAsync(int userId);

    Task<NotificationIdentity?>GetByIdAsync(int notificationId);

    Task SaveChangesAsync();
}