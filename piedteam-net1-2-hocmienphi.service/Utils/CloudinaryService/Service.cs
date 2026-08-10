using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace piedteam_net1_2_hocmienphi.service.Utils.CloudinaryService;

public class Service :MediaService.IService

{
    private readonly Cloudinary _cloudinary;
    private readonly CloudinaryOptions _cloudinaryOptions = new();
    
    public Service(IConfiguration configuration)
    {
        configuration.GetSection(nameof(CloudinaryOptions)).Bind(_cloudinaryOptions);
        _cloudinary = new Cloudinary(new Account(_cloudinaryOptions.CloudName, _cloudinaryOptions.ApiKey, _cloudinaryOptions.ApiSecret));

    }

    public async Task<string> UploadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new Exception("File not exist");
        }
        if(!IsImageFile(file)) throw new Exception("File invalid");
        
        if(!IsValidImageLength(file)) throw new Exception("File invalid length");
        
        //Upload lên cloudinary
        await using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(file.FileName, stream),
        };
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        
        if (uploadResult.SecureUrl == null) throw new Exception("Upload failed");
        return uploadResult.SecureUrl.ToString();
    }
    
    private bool IsValidImageLength(IFormFile file)
    {
        const int maxMegabytes = 5; // toi da 5MB
        long maxBytes = maxMegabytes * 1024 * 1024;

        if (file.Length > maxBytes)
        {
            return false;
        }
        
        
        return true;
    }
    
    private bool IsImageFile(IFormFile file)
    {
        var allowedExtensions = new string[]  { ".jpg", ".jpeg", ".png", ".gif", ".Webp" };

        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        
        
        return allowedExtensions.Contains(fileExtension);
    }
    
    
    
    
}