namespace HireConnect.Profile.DTO;

public class CandidateProfileInternalDTO
{
    public int UserId { get; set; }

    public string FullName { get; set; }
        = string.Empty;

    public string ResumeUrl { get; set; }
        = string.Empty;
}