namespace HireConnect.Profile.Exceptions;

public class ProfileNotFoundException : ProfileException
{
    public ProfileNotFoundException(int userId) 
        : base($"Profile for User ID {userId} not found.", 404) { }
}