
namespace HireConnect.Application.Exceptions;

public class ProfileNotFoundException : ApplicationHireConnectException
{
    public ProfileNotFoundException(string message) : base(message, 404)
    {
    }
}