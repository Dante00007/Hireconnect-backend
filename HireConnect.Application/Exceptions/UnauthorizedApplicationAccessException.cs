
namespace HireConnect.Application.Exceptions;

public class UnauthorizedApplicationAccessException : ApplicationHireConnectException
{
    public UnauthorizedApplicationAccessException(string message) : base(message, 403)
    {
    }
}