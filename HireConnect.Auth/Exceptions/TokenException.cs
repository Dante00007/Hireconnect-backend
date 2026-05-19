namespace HireConnect.Auth.Exceptions;

public class TokenException : AuthException
{
    public TokenException(string message): base(message, 401) { }
}