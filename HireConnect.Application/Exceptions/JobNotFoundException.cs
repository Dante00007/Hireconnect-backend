
namespace HireConnect.Application.Exceptions;
public class JobNotExistException : ApplicationHireConnectException
{
    public JobNotExistException(string message) : base(message, 404)
    {
    }
}