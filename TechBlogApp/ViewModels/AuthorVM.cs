using TechBlogApp.Models;

namespace TechBlogApp.ViewModels
{
    public class AuthorVM
    {
        public List<Article> Article { get; set; }
        public List<Article> PopularPost { get; set; }
        public List<Article> RecentReviews { get; set; }
    }
}
