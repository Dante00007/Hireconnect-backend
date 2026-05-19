namespace HireConnect.Contracts.Events;

public class InterviewCancelledEvent
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
}