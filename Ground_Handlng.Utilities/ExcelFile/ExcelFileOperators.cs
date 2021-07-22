using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ground_Handlng.Utilities.ExcelFile
{
    public class ExcelFileOperators
    {
        public string saveExcel(IFormFile UploadFile)
        {
            var path = Path.Combine(
                       Directory.GetCurrentDirectory(), "UploadExchange",
                       UploadFile.FileName);
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "UploadExchange"))
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "UploadExchange");

            using (var stream = new FileStream(path, FileMode.Create))
            {
                UploadFile.CopyToAsync(stream);
                stream.Close();
            }
            
            return path;
        }
        
    }
}
