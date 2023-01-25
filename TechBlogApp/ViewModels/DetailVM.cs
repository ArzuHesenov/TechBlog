using TechBlogApp.Models;

namespace TechBlogApp.ViewModels
{
    public class DetailVM
    {
        public Article Article { get; set; }
        public List<Article> Suggestions { get; set; }
        public Article Before { get; set; }
        public Article After { get; set; }
    }
}
