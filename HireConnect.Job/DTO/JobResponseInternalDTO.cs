namespace HireConnect.Job.DTO;

public class JobResponseInternalDTO
{
    public int JobId { get; set; }

    public string Title { get; set; }
        = string.Empty;

    public int PostedBy { get; set; }

    public string RecruiterName { get; set; }
        = string.Empty;
}