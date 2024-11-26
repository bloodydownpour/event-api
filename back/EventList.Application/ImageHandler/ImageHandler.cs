using Microsoft.AspNetCore.Http;


namespace EventList.Application.ImageHandler
{
    public class ImageHandler
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

        public void DeleteImage(string root, string path, string fileName)
        {
            var physPath = root + $"/{path}/" + fileName;
            if (File.Exists(physPath))
            {
                File.Delete(physPath);
            }
        }
    }
}
