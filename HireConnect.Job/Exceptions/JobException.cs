namespace HireConnect.Job.Exceptions
{
    public class JobException : Exception
    {
        public int StatusCode { get; }
        public JobException(string message, int statusCode = 400) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}