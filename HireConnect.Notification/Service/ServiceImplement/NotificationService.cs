using HireConnect.Notification.DTO;
using HireConnect.Notification.Hubs;
using HireConnect.Notification.Models;
using HireConnect.Notification.Repository.Interface;
using HireConnect.Notification.Service.Interface;
using Microsoft.AspNetCore.SignalR;

namespace HireConnect.Notification.Service.Implementation;

public class NotificationService
    : INotificationService
{
    private readonly INotificationRepository _repository;
    IHubContext<NotificationHub> _hubContext;

    public NotificationService(
        INotificationRepository repository, IHubContext<NotificationHub> hubContext
    )
    {
        _repository = repository;
        _hubContext = hubContext;
    }

    public async Task CreateNotificationAsync(
        int userId,
        string title,
        string message,
        string? type = null
    )
    {
        Console.WriteLine(
            $"Realtime push for user {userId}"
        );
        Console.WriteLine(
            $"Notification type: {type}"
        );
        var notification =
            new NotificationIdentity
            {
                UserId = userId,

                Title = title,

                Message = message,

                Type = type
            };

        await _repository
            .AddAsync(notification);

        await _repository
            .SaveChangesAsync();

        Console.WriteLine(
            "Sending SignalR notification..."
        );

        await _hubContext
            .Clients
            .Group($"user-{userId}")
            .SendAsync(

                "ReceiveNotification",

                new
                {
                    notification.NotificationId,

                    notification.Title,

                    notification.Message,

                    notification.IsRead,

                    notification.CreatedAt,

                    notification.Type
                }

            );
    }

    public async Task<List<NotificationResponseDTO>>
    GetUserNotificationsAsync(
        int userId
    )
    {
        var notifications =
            await _repository
                .GetUserNotificationsAsync(
                    userId
                );

        return notifications
            .Select(n =>
                new NotificationResponseDTO
                {
                    NotificationId =
                        n.NotificationId,

                    Title = n.Title,

                    Message = n.Message,

                    CreatedAt =
                        n.CreatedAt,

                    IsRead =
                        n.IsRead,

                    Type =
                        n.Type
                }
            )
            .ToList();
    }

    public async Task MarkAsReadAsync(int notificationId, int userId)
    {
        var notification =
            await _repository
                .GetByIdAsync(
                    notificationId
                );

        if (notification == null)
        {
            throw new Exception(
                "Notification not found"
            );
        }

        if (notification.UserId != userId)
        {
            throw new UnauthorizedAccessException();
        }

        notification.IsRead = true;

        await _repository
            .SaveChangesAsync();
    }
}