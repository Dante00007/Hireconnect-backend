namespace HireConnect.Interview.DTO;

public class InterviewSummaryInternalDTO
{
    public int InterviewId
    {
        get;
        set;
    }

    public int ApplicationId
    {
        get;
        set;
    }

    public string CandidateName
    {
        get;
        set;
    } = string.Empty;

    public string JobTitle
    {
        get;
        set;
    } = string.Empty;

    public DateTime ScheduledAt
    {
        get;
        set;
    }

    public string Status
    {
        get;
        set;
    } = string.Empty;
}