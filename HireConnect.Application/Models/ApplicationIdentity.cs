using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HireConnect.Application.Models
{
    [Table("Applications")]
    public class ApplicationIdentity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicationId { get; set; }
        [Required]
        public int JobId { get; set; }
        [Required]
        public int CandidateId { get; set; }
        // New Denormalized Fields
        [Required]
        public string CandidateName { get; set; } = string.Empty;
        [Required]
        [StringLength(200)]
        public string JobTitle { get; set; } = string.Empty;
        [Required]
        [StringLength(200)]
        public string RecruiterName { get; set; } = string.Empty;
        [Required]
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow; 
        [Required]
        public string Status { get; set; } = ApplicationStatus.Applied.ToString();
        public string? CoverLetter { get; set; }
        [Required]
        public string ResumeUrl { get; set; } = string.Empty; 
    }
}