using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechBlogApp.Areas.Admin.ViewModels;
using TechBlogApp.Models;
using TechBlogApp.Services;

namespace TechBlogApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IWebHostEnvironment _environment;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContext, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContext = httpContext;
            _environment = environment;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var user = _userManager.Users.ToList();
            return View(user);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRole(string id)
        {
            if (id == null) return NotFound();
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userRoles = (await _userManager.GetRolesAsync(user)).ToList();
            var roles = _roleManager.Roles.Select(role => role.Name).ToList();

            UserRoleAddVM vm = new()
            {
                User = user,
                Roles = roles.Except(userRoles)
            };
            return View(vm);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRole(string id, string role)
        {
            if (id == null) return NotFound();
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userAddRole = await _userManager.AddToRoleAsync(user, role);
            if (!userAddRole.Succeeded)
            {
                return View();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UserInfo()
        {
            var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            User user=await _userManager.FindByIdAsync(userId);

            return View(user);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UserInfo(User user,IFormFile Photo)
        {
            var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            User findUser = await _userManager.FindByIdAsync(userId);
            var photoUrl = "";
            if (Photo==null)
            {
                photoUrl = "/uploads/avatar.png";
            }
            else
            {
                photoUrl = ImageService.UploadImage(Photo, _environment);
            }
            try
            {
                findUser.PhotoUrl = photoUrl;
                findUser.FirstName = user.FirstName;
                findUser.LastName = user.LastName;
                findUser.AboutAuthor = user.AboutAuthor;
                await _userManager.UpdateAsync(findUser);
            }
            catch (Exception)
            {
                return View();
            }

            return RedirectToAction("Index","Home");
        }
    }
}
