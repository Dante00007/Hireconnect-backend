namespace HireConnect.Job.DTO;

public class JobSummaryInternalDTO
{
    public int JobId { get; set; }

    public string Title
    {
        get;
        set;
    } = string.Empty;

    public string Category
    {
        get;
        set;
    } = string.Empty;

    public string Type
    {
        get;
        set;
    } = string.Empty;

    public string Location
    {
        get;
        set;
    } = string.Empty;

    public DateTime CreatedAt
    {
        get;
        set;
    }
}