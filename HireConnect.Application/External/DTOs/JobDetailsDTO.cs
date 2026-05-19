namespace HireConnect.Application.External.DTOs;

public class JobDetailsDTO
{
    public int JobId { get; set; }

    public string Title { get; set; }
        = string.Empty;

    public int PostedBy { get; set; }

    public string RecruiterName { get; set; }
        = string.Empty;
}