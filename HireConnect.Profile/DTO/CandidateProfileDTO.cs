

namespace HireConnect.Profile.DTO;


public class CandidateProfileDTO
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Mobile { get; set; } = string.Empty;
    public DateOnly DOB { get; set; }
    public string Skills { get; set; } = string.Empty;
    public int Experience { get; set; }
    public IFormFile? Resume { get; set; }
    public IFormFile? ProfileImage {get;set;}

    public AddressDTO Address { get; set; } = new AddressDTO();

    public CandidateProfileDTO() { }

    public CandidateProfileDTO(string fullName, string mobile, DateOnly dob, string skills, int experience, AddressDTO address)
    {
        FullName = fullName;
        Mobile = mobile;
        DOB = dob;
        Skills = skills;
        Experience = experience;

        Address = address;
    }

}
