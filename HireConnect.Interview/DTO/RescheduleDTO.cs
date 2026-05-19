namespace HireConnect.Interview.DTO;

public class RescheduleDto
{
    public DateTime NewDateTime
    {
        get;
        set;
    }

    public string? MeetLink
    {
        get;
        set;
    }

    public string? Location
    {
        get;
        set;
    }

    public string? Notes
    {
        get;
        set;
    }
}