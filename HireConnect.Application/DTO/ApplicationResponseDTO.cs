namespace HireConnect.Application.DTO
{
    public class ApplicationResponseDTO
    {
        public int ApplicationId { get; set; }

        public int JobId { get; set; }

        public int CandidateId { get; set; }

        public string CandidateName { get; set; } = string.Empty;
        public string JobTitle { get; set; }
            = string.Empty;

        public string RecruiterName { get; set; }
            = string.Empty;

        public DateTime AppliedAt { get; set; }

        public string Status { get; set; }
            = string.Empty;

        public string? CoverLetter { get; set; }

        public string ResumeUrl { get; set; }
            = string.Empty;
    }
}