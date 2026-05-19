

namespace HireConnect.Profile.DTO
{
    public class UserProfileDTO
    {
        public int ProfileId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty; 

        public string? Mobile { get; set; }
        public DateOnly? DOB { get; set; }
        public string? Skills { get; set; }
        public int? Experience { get; set; }
        public string? ResumeUrl { get; set; }

        public string? CompanyName { get; set; }
        public string? CompanySize { get; set; }
        public string? Industry { get; set; }
        public string? Website { get; set; }
        public string? ProfileImageUrl {get;set;}

        public string? CompanyLogoUrl {get;set;}
        public AddressDTO? Address { get; set; }

    }
}