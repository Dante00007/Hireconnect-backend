namespace HireConnect.Interview.DTO;

public class InterviewResponseDTO
{
    public int InterviewId { get; set; }

    public int ApplicationId { get; set; }

    public int CandidateId { get; set; }

    public int RecruiterId { get; set; }

    public string CandidateName { get; set; }
        = string.Empty;

    public string JobTitle { get; set; }
        = string.Empty;

    public DateTime ScheduledAt { get; set; }

    public string InterviewMode { get; set; }
        = string.Empty;

    public string? MeetLink { get; set; }

    public string? Location { get; set; }

    public string Status { get; set; }
        = string.Empty;

    public string? Notes { get; set; }

    public int DurationMinutes { get; set; }
}