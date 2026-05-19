namespace HireConnect.Interview.External.DTO;

public class ApplicationInternalResponseDTO
{
    public int ApplicationId { get; set; }
    public int CandidateId { get; set; }
    public string CandidateName { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
}
