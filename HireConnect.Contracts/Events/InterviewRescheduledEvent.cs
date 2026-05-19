namespace HireConnect.Contracts.Events;

public class InterviewRescheduledEvent
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

    public DateTime NewDateTime
    {
        get;
        set;
    }
}