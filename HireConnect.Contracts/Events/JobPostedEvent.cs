namespace HireConnect.Contracts.Events;

public class JobPostedEvent
{
    public int JobId
    {
        get;
        set;
    }

    public string JobTitle
    {
        get;
        set;
    } = string.Empty;

    public string CompanyName
    {
        get;
        set;
    } = string.Empty;
}