using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HireConnect.Profile.Models
{
    public class RecruiterProfile
    {
        [Key]
        public int ProfileId { get; set; } 

        [Required]
        public int UserId { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty; 

        [Required]
        public string CompanyName { get; set; } = string.Empty;

        public string CompanySize { get; set; } = string.Empty;

        public string Industry { get; set; } = string.Empty;

        public string Website { get; set; } = string.Empty;
        public string CompanyLogoUrl {get;set;} = string.Empty;
        // Link to Address
        public int AddressId { get; set; }
        [ForeignKey("AddressId")]
        public Address? Address { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}