namespace HireConnect.Job.DTO;

public class JobResponseDTO
{
    public int JobId { get; set; }

    public string Title { get; set; }
        = string.Empty;

    public string Category { get; set; }
        = string.Empty;

    public string Type { get; set; }
        = string.Empty;

    public string Location { get; set; }
        = string.Empty;

    public double SalaryMin { get; set; }

    public double SalaryMax { get; set; }

    public string Description { get; set; }
        = string.Empty;

    public List<string> Skills { get; set; }
        = new();

    public int ExperienceRequired { get; set; }

    // Recruiter Info
    public int PostedBy { get; set; }

    public string PostedByName { get; set; }
        = string.Empty;
    
    public string Status { get; set; } = string.Empty;

    // Optional future UI usage
    public DateTime CreatedAt { get; set; }
}