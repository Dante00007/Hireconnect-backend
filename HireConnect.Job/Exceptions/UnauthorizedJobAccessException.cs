namespace HireConnect.Job.Exceptions
{
    public class UnauthorizedJobAccessException : JobException
    {
        public UnauthorizedJobAccessException(int recruiterId, int jobId) 
            : base($"Recruiter {recruiterId} is not authorized to modify Job {jobId}.", 401) { }
    }
}