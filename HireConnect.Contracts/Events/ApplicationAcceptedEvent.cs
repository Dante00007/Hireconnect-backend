namespace HireConnect.Contracts.Events;

public class ApplicationAcceptedEvent
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