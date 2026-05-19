
namespace HireConnect.Profile.Exceptions;
public class ProfileAlreadyExistsException : ProfileException
{
    public ProfileAlreadyExistsException(string type) 
        : base($"{type} profile already exists for this user.", 409) { }
}