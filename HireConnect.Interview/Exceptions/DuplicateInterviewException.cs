namespace HireConnect.Interview.Exceptions;

public class DuplicateInterviewException : InterviewException
{
    public DuplicateInterviewException(string message) : base(message, 409)
    {
    }
}