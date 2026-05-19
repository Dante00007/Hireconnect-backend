namespace HireConnect.Contracts.Events;

public class InterviewCompletedEvent
{
    public int CandidateId
    {
        get;
        set;
    }

    public int RecruiterId
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