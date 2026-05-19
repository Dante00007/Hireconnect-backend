namespace HireConnect.Interview.Exceptions;

public class InterviewNotFoundException : InterviewException
{
    public InterviewNotFoundException(string message) : base(message,404)
    {
    }
}