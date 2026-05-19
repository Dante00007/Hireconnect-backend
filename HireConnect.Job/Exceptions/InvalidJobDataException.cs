namespace HireConnect.Job.Exceptions
{
    public class InvalidJobDataException : JobException
    {
        public InvalidJobDataException(string message) : base(message, 400) { }
    }
}