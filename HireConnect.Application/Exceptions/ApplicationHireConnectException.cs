namespace HireConnect.Application.Exceptions;

public class ApplicationHireConnectException : Exception
{
    public int StatusCode { get; set; }
    public ApplicationHireConnectException(string message, int statusCode=400) : base(message)
    {
        StatusCode = statusCode;
    }
}