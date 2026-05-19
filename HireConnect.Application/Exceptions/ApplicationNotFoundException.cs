namespace HireConnect.Application.Exceptions;

public class ApplicationNotFoundException : ApplicationHireConnectException
{
    public ApplicationNotFoundException(string message) : base(message, 404)
    {
    }
}