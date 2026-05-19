

namespace HireConnect.Profile.DTO;
public class RecruiterProfileDTO
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string CompanySize { get; set; } = string.Empty;
    public IFormFile? CompanyLogo {get;set; }

    public string Industry { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    
    public AddressDTO Address { get; set; } = new AddressDTO();
    public RecruiterProfileDTO() { }
    public RecruiterProfileDTO(string fullName,string email, string companyName, string companySize, string industry, string website,AddressDTO address)
    {
        FullName = fullName;
        Email = email;
        CompanyName = companyName;
        CompanySize = companySize;
        Industry = industry;
        Website = website;
        Address = address;
    }
}