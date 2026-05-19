using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using HireConnect.Profile.Exceptions;
using HireConnect.Profile.Service.Interface;

public class FileServiceImpl : IFileService
{
    private readonly Cloudinary _cloudinary;

    public FileServiceImpl(IConfiguration config)
    {
        var acc = new Account(
            config["CloudinarySettings:CloudName"],
            config["CloudinarySettings:ApiKey"],
            config["CloudinarySettings:ApiSecret"]
        );
        _cloudinary = new Cloudinary(acc);
    }


    public async Task<ImageUploadResult> UploadImageAsync(IFormFile file, string folderName)
    {
        if (file == null || file.Length <= 0)
        {
            throw new FileUploadException(
                "Image file is empty."
            );
        }
        var allowedTypes = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        if (!ValidateFileType(file, allowedTypes))
        {
            throw new FileUploadException(
                "Invalid image type. Only JPG, PNG, and GIF files are allowed."
            );
        }
        using (var stream =
            file.OpenReadStream())
        {

            var uploadParams =
                new ImageUploadParams
                {
                    File =
                        new FileDescription(
                            file.FileName,
                            stream
                        ),

                    Folder = folderName,

                    AccessMode = "public"
                };

            var uploadResult =
                await _cloudinary
                    .UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new FileUploadException(
                    $"Cloudinary Image Upload Failed: {uploadResult.Error.Message}"
                );
            }

            return uploadResult;
        }
    }
    public async Task<RawUploadResult> UploadDocumentAsync(IFormFile file, string folderName)
    {
        if (file == null || file.Length <= 0)
        {
            throw new FileUploadException(
                "Document file is empty."
            );
        }
        var allowedTypes = new[] { ".pdf", ".doc", ".docx" };
        if (!ValidateFileType(file, allowedTypes))
        {
            throw new FileUploadException(
                "Invalid document type. Only PDF and Word documents are allowed."
            );
        }

        using (var stream = file.OpenReadStream())
        {

            var uploadParams =
                new RawUploadParams
                {
                    File =
                        new FileDescription(
                            file.FileName,
                            stream
                        ),

                    Folder = folderName,

                    AccessMode = "public",

                    UseFilename = true,

                    UniqueFilename = false,

                    Overwrite = true
                };

            var uploadResult =
                await _cloudinary
                    .UploadAsync(uploadParams);


            return uploadResult;
        }
    }
    public async Task<DeletionResult> DeleteFileAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        return await _cloudinary.DestroyAsync(deleteParams);
    }

    private bool ValidateFileType(IFormFile file, string[] allowedTypes)
    {
        var fileExtension = Path.GetExtension(file.FileName).ToLower();
        return allowedTypes.Contains(fileExtension);
    }
}

