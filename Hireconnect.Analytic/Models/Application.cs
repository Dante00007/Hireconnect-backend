namespace Hireconnect.Analytic.Models;

public class Application
{
    public int ApplicationId { get; set; }
    public int JobId { get; set; }
    public Job Job { get; set; } = null!;
    public string Status { get; set; } = string.Empty; // e.g., "Offered", "Rejected"
    public DateTime AppliedAt { get; set; }
    public DateTime StatusChangedAt { get; set; }
}