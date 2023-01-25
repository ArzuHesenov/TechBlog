using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechBlogApp.Data;
using TechBlogApp.Models;

namespace TechBlogApp.Controllers
{
    public class AuthorController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;

        public AuthorController(UserManager<User> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index(int id,string userId)
        {
            var article=_context.Articles.Where(a => a.Id == id).Include(x=>x.UserId==userId);
            return View();
        }
    }
}
