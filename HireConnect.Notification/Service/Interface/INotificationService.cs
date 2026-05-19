using HireConnect.Notification.DTO;

namespace HireConnect.Notification.Service.Interface;

public interface INotificationService
{
    Task CreateNotificationAsync(int userId,string title,string message,string? type = null); 
    Task<List<NotificationResponseDTO>>GetUserNotificationsAsync(int userId);

    Task MarkAsReadAsync(int notificationId,int userId);
}