using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HireConnect.Notification.Models;

[Table("Notifications")]
public class NotificationIdentity
{
    [Key]
    public int NotificationId
    {
        get;
        set;
    }

    [Required]
    public int UserId
    {
        get;
        set;
    }

    [Required]
    public string Title
    {
        get;
        set;
    } = string.Empty;

    [Required]
    public string Message
    {
        get;
        set;
    } = string.Empty;

    [Required]
    public bool IsRead
    {
        get;
        set;
    } = false;

    [Required]
    public DateTime CreatedAt
    {
        get;
        set;
    } = DateTime.UtcNow;

    public string? Type
    {
        get;
        set;
    }
}