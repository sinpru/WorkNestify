using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace WorkNestify.Services;

public class CloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IConfiguration configuration)
    {
        var account = new Account(
            configuration["Services:Cloudinary:CloudName"],
            configuration["Services:Cloudinary:ApiKey"],
            configuration["Services:Cloudinary:ApiSecret"]
        );
        _cloudinary = new Cloudinary(account);
    }
    
    public async Task<string> UploadImageAsync(IFormFile file)
    {
        // Create ImageUploadParams to prepare for upload destination
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            PublicId = $"images/{Guid.NewGuid()}",
            Folder = "worknestify/images"
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new Exception($"Upload image failed: {uploadResult.Error?.Message}");
        }
        return uploadResult.SecureUrl.AbsoluteUri;
    }
    
    public async Task<bool> DeleteImageAsync(string existingLogoUrl)
    {
        try
        {
            if (string.IsNullOrEmpty(existingLogoUrl)) return false;

            // Extract PublicId from existing Cloudinary URL
            var uri = new Uri(existingLogoUrl);
            string path = uri.AbsolutePath;
            var segments = path.Split('/');

            // Delete the old image from Cloudinary
            int uploadIndex = Array.FindIndex(segments, s => s == "upload");
            if (uploadIndex < 0 || uploadIndex + 2 >= segments.Length)
            {
                return false;
            }
            
            // Extract the PublicId (everything after the version number)
            var publicIdSegments = segments.Skip(uploadIndex + 2);
            string publicId = string.Join("/", publicIdSegments); 
            publicId = Path.ChangeExtension(publicId, null);

            // Delete the image from Cloudinary
            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Image
            };
            var deleteResult = await _cloudinary.DestroyAsync(deleteParams);
            return deleteResult.Result == "ok";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting image: {ex.Message}");
            return false;
        }
    }
    
    public async Task<string> UploadDocumentAsync(IFormFile file)
    {
        // Define allowed file extensions
        var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".odt", ".rtf", ".txt" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

        // Validate file extension
        if (string.IsNullOrEmpty(fileExtension) || !allowedExtensions.Contains(fileExtension))
        {
            throw new ArgumentException("Invalid file type. Only .pdf, .doc, .docx, .odt, .rtf, and .txt files are allowed.");
        }

        // Validate file size (optional, e.g., max 10MB)
        if (file.Length > 10 * 1024 * 1024) // 10MB
        {
            throw new ArgumentException("File size exceeds the maximum limit of 10MB.");
        }

        // Use RawUploadParams for flexibility
        var uploadParams = new RawUploadParams
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            PublicId = $"documents/{Guid.NewGuid()},",
            Folder = "worknestify/documents"
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new Exception($"Upload document failed: {uploadResult.Error?.Message}");
        }

        return uploadResult.SecureUrl.AbsoluteUri;
    }
    
    public async Task<bool> DeleteDocumentAsync(string existingDocumentUrl)
    {
        try
        {
            if (string.IsNullOrEmpty(existingDocumentUrl)) return false;

            // Extract PublicId from Cloudinary URL
            var uri = new Uri(existingDocumentUrl);
            string path = uri.AbsolutePath;
            var segments = path.Split('/');

            // Find the index of "upload" and the version number
            int uploadIndex = Array.FindIndex(segments, s => s == "upload");
            if (uploadIndex < 0 || uploadIndex + 2 >= segments.Length)
            {
                return false; // Invalid URL structure
            }
            
            // Extract the PublicId (everything after the version number)
            var publicIdSegments = segments.Skip(uploadIndex + 2);
            string publicId = string.Join("/", publicIdSegments);
            // publicId = Path.ChangeExtension(publicId, null);

            // Delete the document from Cloudinary
            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Raw
            };
            var deleteResult = await _cloudinary.DestroyAsync(deleteParams);
            return deleteResult.Result == "ok";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting document: {ex.Message}");
            return false;
        }
    }
}