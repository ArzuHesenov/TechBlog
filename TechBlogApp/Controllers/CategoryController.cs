using Microsoft.AspNetCore.Mvc;

namespace TechBlogApp.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
