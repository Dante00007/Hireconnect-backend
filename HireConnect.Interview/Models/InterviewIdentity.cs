using System.ComponentModel.DataAnnotations;

namespace HireConnect.Interview.Models
{
    public class InterviewIdentity
    {
        [Key]
        public int InterviewId { get; set; }

        [Required]
        public int ApplicationId { get; set; }
        [Required]
        public int CandidateId { get; set; }
        [Required]
        public int RecruiterId { get; set; }

        [Required]
        public string CandidateName { get; set; } = string.Empty;
        [Required]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        public DateTime ScheduledAt { get; set; }

        [Required]
        [StringLength(50)]
        public string InterviewMode { get; set; } = "Online"; // e.g., "Online" or "In-Person" 
        public int DurationMinutes{get; set;} = 60;
        public string? MeetLink { get; set; } // 

        public string? Location { get; set; } // 

        [Required]
        public string Status { get; set; } = InterviewStatus.Scheduled.ToString(); // 

        public string? Notes { get; set; } // 
    }
}