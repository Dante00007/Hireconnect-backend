namespace Hireconnect.Analytic.Models;
 public class Job
    {
        public int JobId { get; set; }
        public string Category { get; set; } = string.Empty;
        public int PostedBy { get; set; } // Recruiter ID
    }