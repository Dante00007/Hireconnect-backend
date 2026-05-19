namespace HireConnect.Contracts.Events;

public class InterviewScheduledEvent
{
    public int CandidateId
    {
        get;
        set;
    }

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
}