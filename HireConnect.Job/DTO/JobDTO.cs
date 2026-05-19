using System.ComponentModel.DataAnnotations;

namespace HireConnect.Job.DTO;

public class JobDTO
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Category { get; set; } = string.Empty;

    [Required]
    public string Type { get; set; } = string.Empty;

    [Required]
    public string Location { get; set; } = string.Empty;

    public double SalaryMin { get; set; }

    public double SalaryMax { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;

    public List<string> Skills { get; set; } = new();

    public int ExperienceRequired { get; set; }
}