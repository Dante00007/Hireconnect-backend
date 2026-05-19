namespace HireConnect.Contracts.Events;

public class ApplicationInterviewEvent
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