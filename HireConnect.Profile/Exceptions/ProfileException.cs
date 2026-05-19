
namespace HireConnect.Profile.Exceptions;

public class ProfileException : Exception
{
    public int StatusCode { get; }
    public ProfileException(string message, int statusCode = 400) : base(message)
    {
        StatusCode = statusCode;
    }
}