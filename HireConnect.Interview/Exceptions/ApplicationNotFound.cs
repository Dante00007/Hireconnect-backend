
namespace HireConnect.Interview.Exceptions;

public class ApplicationNotFoundException : InterviewException
{
    public ApplicationNotFoundException(string message) : base(message, 404)
    {
    }
}