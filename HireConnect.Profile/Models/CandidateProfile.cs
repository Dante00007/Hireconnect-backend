using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HireConnect.Profile.Models
{
    public class CandidateProfile
    {
        [Key]
        public int ProfileId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Mobile { get; set; } = string.Empty;
        public DateOnly DOB { get; set; }

        public string Skills { get; set; } = string.Empty;

        public int Experience { get; set; }

        public string ResumeUrl { get; set; } = string.Empty;
        public string ProfileImageUrl {get;set;} = string.Empty;
        public int AddressId { get; set; }
        [ForeignKey("AddressId")]
        public Address? Address { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}