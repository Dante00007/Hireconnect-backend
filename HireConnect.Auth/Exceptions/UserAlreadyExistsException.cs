namespace HireConnect.Auth.Exceptions;
public class UserAlreadyExistsException : AuthException
{
    public UserAlreadyExistsException(string email)
        : base($"User with email '{email}' already exists.", 409) { }
}