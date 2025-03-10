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
            configuration["Services:Cloudinary:CloudName"],
            configuration["Services:Cloudinary:ApiKey"],
            configuration["Services:Cloudinary:ApiSecret"]
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
            if (uploadIndex >= 0 && uploadIndex + 2 < segments.Length)
            {
                var publicIdSegments = segments.Skip(uploadIndex + 2);
                string publicId = string.Join("/", publicIdSegments); 
                publicId = Path.ChangeExtension(publicId, null);

                var deleteParams = new DeletionParams(publicId);
                var deleteResult = await _cloudinary.DestroyAsync(deleteParams);
                return deleteResult.Result == "ok";
            }
            
            return false;
        }
        catch
        {
            return false;
        }
    }
}