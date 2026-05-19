namespace HireConnect.Application.External.DTOs;

public class CandidateProfileDTO
{
    public int UserId { get; set; }

    public string FullName { get; set; }
        = string.Empty;

    public string ResumeUrl { get; set; }
        = string.Empty;
}