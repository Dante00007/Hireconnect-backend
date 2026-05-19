namespace HireConnect.Auth.Exceptions;
public class InvalidCredentialException : AuthException
    {
        public InvalidCredentialException() 
            : base("Invalid email or password.", 401) { }
    }