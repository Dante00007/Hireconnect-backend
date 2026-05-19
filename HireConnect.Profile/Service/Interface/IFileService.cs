using CloudinaryDotNet.Actions;

namespace HireConnect.Profile.Service.Interface
{
    public interface IFileService
    {
        Task<ImageUploadResult> UploadImageAsync(IFormFile file,string folderName);
        Task<RawUploadResult> UploadDocumentAsync(IFormFile file,string folderName);
        Task<DeletionResult> DeleteFileAsync(string publicId);
    }
}