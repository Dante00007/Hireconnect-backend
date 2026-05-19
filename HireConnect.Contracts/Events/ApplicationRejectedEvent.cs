namespace HireConnect.Contracts.Events;

public class ApplicationRejectedEvent
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