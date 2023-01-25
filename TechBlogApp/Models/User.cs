using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TechBlogApp.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhotoUrl { get; set; }
        public string AboutAuthor { get; set; }
        public List<Article> Articles { get; set; }
        public List<UserAds>  UserAds { get; set; }
    }
}
