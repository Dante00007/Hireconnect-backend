namespace HireConnect.Job.Exceptions
{
    public class JobAlreadyClosedException : JobException
    {
        public JobAlreadyClosedException(int jobId) 
            : base($"Operation failed. Job {jobId} is already closed.", 400) { }
    }
}