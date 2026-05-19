namespace HireConnect.Analytic.DTO;

public class RecentApplicationDTO
{
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