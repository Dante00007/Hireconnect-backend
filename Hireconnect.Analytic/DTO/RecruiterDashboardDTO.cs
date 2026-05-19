namespace HireConnect.Analytic.DTO;
public class RecruiterDashboardDTO
{
    public int TotalJobs { get; set; }

    public int ActiveJobs { get; set; }

    public int TotalApplications { get; set; }

    public int TotalInterviews { get; set; }

    public int AcceptedCandidates { get; set; }

    public int ScheduledInterviews { get; set; }

    public Dictionary<string, int>
        ApplicationStatusBreakdown
    {
        get;
        set;
    } = new();

    public List<RecentApplicationDTO>
        RecentApplications
    {
        get;
        set;
    } = new();
}