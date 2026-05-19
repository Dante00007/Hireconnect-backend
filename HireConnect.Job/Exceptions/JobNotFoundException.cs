namespace HireConnect.Job.Exceptions
{
    public class JobNotFoundException : JobException
    {
        public JobNotFoundException(int jobId) 
            : base($"Job with ID {jobId} was not found.", 404) { }
    }
}