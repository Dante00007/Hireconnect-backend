using System.ComponentModel.DataAnnotations;

namespace HireConnect.Job.Models;


public class Jobs
{
    [Key]
    public int JobId { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Category { get; set; } = string.Empty;

    [Required]
    public string Type { get; set; } = string.Empty; // e.g., Full-time, Remote 

    [Required]
    public string Location { get; set; } = string.Empty;

    public double SalaryMin { get; set; }


    public double SalaryMax { get; set; }


    [Required]
    public string Description { get; set; } = string.Empty;

    public List<string> Skills { get; set; } = new();

    public int ExperienceRequired { get; set; }


    [Required]
    public int PostedBy { get; set; }

    [Required]
    public string PostedByName { get; set; } = string.Empty;

    [Required]
    public string Status { get; set; } = "Active";

    public DateTime PostedAt { get; set; } = DateTime.UtcNow;
}