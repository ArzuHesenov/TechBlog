using TechBlogApp.Models;

namespace TechBlogApp.Areas.Admin.ViewModels
{
    public class UserRoleAddVM
    {
        public User User { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
