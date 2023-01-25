using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechBlogApp.Data;

namespace TechBlogApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public RoleController(RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole identityRole)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var findRole = await _roleManager.FindByNameAsync(identityRole.Name);
            if (findRole != null)
            {
                ViewBag.FindRole = "Role is exist";
                return View();
            }
            await _roleManager.CreateAsync(identityRole);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var result = _roleManager.Roles.FirstOrDefault(x => x.Id == id);
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(IdentityRole identityRole)
        {
            var findRole = _context.Roles.FirstOrDefault(x => x.Name == identityRole.Name);
            if (findRole != null)
            {
                return View(findRole);
            }
            _context.Roles.Update(identityRole);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(string id)
        {
            var delete = _roleManager.Roles.FirstOrDefault(x => x.Id == id);
            return View(delete);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(IdentityRole identityRole)
        {
            await _roleManager.DeleteAsync(identityRole);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
