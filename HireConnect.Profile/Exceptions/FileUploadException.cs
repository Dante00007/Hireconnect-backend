namespace HireConnect.Profile.Exceptions;

public class FileUploadException : ProfileException
{
    public FileUploadException(string message) 
        : base(message, 500) { }
}