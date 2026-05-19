namespace HireConnect.Contracts.Events;

public class ApplicationSubmittedEvent
{
    public int RecruiterId
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
}