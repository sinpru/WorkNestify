using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace WorkNestify.Utilities.Services;

public class CloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IConfiguration configuration)
    {
        var account = new Account(
            configuration["Cloudinary:CloudName"],
            configuration["Cloudinary:ApiKey"],
            configuration["Cloudinary:ApiSecret"]
        );
        _cloudinary = new Cloudinary(account);
    }
    
    public async Task<string> UploadImageAsync(IFormFile file)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            PublicId = $"companies/{Guid.NewGuid()}"
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        return uploadResult.SecureUrl.AbsoluteUri;
    }
    
    public async Task<bool> DeleteImageAsync(string existingLogoUrl)
    {
        try
        {
            if (string.IsNullOrEmpty(existingLogoUrl)) return false;

            // Extract PublicId from existing Cloudinary URL
            var uri = new Uri(existingLogoUrl);
            string publicId = Path.GetFileNameWithoutExtension(uri.AbsolutePath);

            // Delete the old image from Cloudinary
            var deleteParams = new DeletionParams(publicId);
            var deleteResult = await _cloudinary.DestroyAsync(deleteParams);
            return deleteResult.Result == "ok";
        }
        catch
        {
            return false;
        }
    }
}