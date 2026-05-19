namespace HireConnect.Interview.Exceptions;

public class InterviewException : Exception
{
    public int StatusCode { get; set; }
    public InterviewException(string message,int statuscode=400) : base(message)
    {
        StatusCode = statuscode;   
    }
}
