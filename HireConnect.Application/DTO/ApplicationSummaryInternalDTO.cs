namespace HireConnect.Application.DTO;

public class ApplicationSummaryInternalDTO
{
    public int ApplicationId
    {
        get;
        set;
    }

    public int JobId
    {
        get;
        set;
    }

    public int CandidateId
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

    public string Status
    {
        get;
        set;
    } = string.Empty;

    public DateTime AppliedAt
    {
        get;
        set;
    }
}