
namespace HireConnect.Application.Exceptions;
public class DuplicateException : ApplicationHireConnectException
{
    public DuplicateException(string message) : base(message, 409)
    {
    }
}