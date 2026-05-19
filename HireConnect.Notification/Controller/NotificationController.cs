using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using HireConnect.Notification.Service.Interface;

namespace HireConnect.Notification.Controllers;

[ApiController]
[Route("api/notification")]
[Authorize]
public class NotificationController
    : ControllerBase
{
    private readonly INotificationService
        _notificationService;

    public NotificationController(
        INotificationService notificationService
    )
    {
        _notificationService =
            notificationService;
    }

    [HttpGet]
    public async Task<ActionResult>
    GetMyNotifications()
    {
        var userId =
            int.Parse(
                User.FindFirst(
                    ClaimTypes.NameIdentifier
                )!.Value
            );

        var notifications =
            await _notificationService
                .GetUserNotificationsAsync(
                    userId
                );
        if(notifications == null)
        {
            return NotFound();
        }

        return Ok(notifications);
    }

    [HttpPut("{notificationId}/read")]
    public async Task<ActionResult>
    MarkAsRead(int notificationId)
    {
        var userId =
            int.Parse(
                User.FindFirst(
                    ClaimTypes.NameIdentifier
                )!.Value
            );

        await _notificationService
            .MarkAsReadAsync(
                notificationId,
                userId
            );

        return NoContent();
    }
}