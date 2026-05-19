namespace HireConnect.Notification.DTO;

public class NotificationResponseDTO
{
    public int NotificationId
    {
        get;
        set;
    }

    public string Title
    {
        get;
        set;
    } = string.Empty;

    public string Message
    {
        get;
        set;
    } = string.Empty;

    public bool IsRead
    {
        get;
        set;
    }

    public DateTime CreatedAt
    {
        get;
        set;
    }

    public string? Type
    {
        get;
        set;
    }
}