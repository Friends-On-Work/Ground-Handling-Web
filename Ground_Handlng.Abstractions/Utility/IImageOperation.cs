using Microsoft.AspNetCore.Http;

namespace Ground_Handlng.Abstractions.Utility
{
    public interface IImageOperation
    {
        string ImageUpload(IFormFile ImageFile, string DirectoryName, string fileNamePrefix = "");
    }
}
