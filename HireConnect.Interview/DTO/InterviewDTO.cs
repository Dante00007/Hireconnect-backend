namespace HireConnect.Interview.DTO;

public class InterviewDto
{
    public int ApplicationId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string InterviewMode { get; set; } = "Online";// Online or In-Person 
    public string? MeetLink { get; set; }
    public int DurationMinutes {get;set;} = 60;
    public string? Location { get; set; }
    public string? Notes { get; set; }
}