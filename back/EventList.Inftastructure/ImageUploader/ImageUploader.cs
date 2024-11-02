using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;


namespace EventList.Infrastructure.ImageUploader
{
    public class ImageUploader
    {
        public async Task<string> UploadImage(string root, string path, IFormFile file)
        {
            string fileName;
            if (file != null && file.Length > 0)
            {
                fileName = file.FileName;
                var physPath = root + $"/{path}/" + file.FileName;
                using var stream = new FileStream(physPath, FileMode.Create);
                await file.CopyToAsync(stream);
            }
            else
            {
                fileName = "default.png";
            }
            return fileName;
        }
    }
}
