namespace  HireConnect.Interview.Exceptions;

public class MeetingException : InterviewException
{
    public MeetingException(string message) : base(message, 400)
    {
    }
}