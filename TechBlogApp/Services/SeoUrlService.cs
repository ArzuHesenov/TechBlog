using TechBlogApp.Models;

namespace TechBlogApp.Services
{
    public static class SeoUrlService
    {
        public static string SeoUrl(string url)
        {
            var result = url.ToLower()
                .Replace("ə", "e")
                .Replace("ü", "u")
                .Replace("ı", "i")
                .Replace("ö", "o")
                .Replace("ğ", "g")
                .Replace("ç", "c")
                .Replace("ş", "s")
                .Replace(" ", "-")
                .Replace(",", "-")
                .Replace(".", "");


            return result;
        }
    }
}
