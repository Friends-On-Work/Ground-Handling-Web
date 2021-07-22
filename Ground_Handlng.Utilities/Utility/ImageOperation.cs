using Ground_Handlng.Abstractions.Utility;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Ground_Handlng.Utilities.Utility
{
    public class ImageOperation : IImageOperation
    {
        public string ImageUpload(IFormFile ImageFile, string DirectoryName, string fileNamePrefix = "")
        {
            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\images\\" + DirectoryName, fileNamePrefix + ImageFile.FileName);

            if (!Directory.Exists(Directory.GetCurrentDirectory() + "wwwroot\\images\\" + DirectoryName))
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "wwwroot\\images\\" + DirectoryName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                ImageFile.CopyTo(stream);
            }
            return path;
        }
    }
}
