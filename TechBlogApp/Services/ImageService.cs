namespace TechBlogApp.Services
{
    public static class ImageService
    {
        public static string UploadImage(IFormFile image, IWebHostEnvironment _web)
        {
            var path = "/uploads/" + Guid.NewGuid() + image.FileName;
            using (var fileStream = new FileStream(_web.WebRootPath + path, FileMode.Create))
            {
                image.CopyTo(fileStream);
            }
            return path;
        }
    }
}
