using Microsoft.AspNetCore.Mvc;
using TechBlogApp.Data;
using TechBlogApp.Models;

namespace TechBlogApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly AppDbContext _context;

        public TagController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var tag = _context.Tags.ToList();
            return View(tag);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Tag tag)
        {
            var findTag = _context.Tags.FirstOrDefault(x => x.Name == tag.Name);
            if (findTag != null)
            {
                ViewBag.TagExist = "This tag is exist";
                return View(findTag);
            }
            _context.Tags.Add(tag);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var tag = _context.Tags.FirstOrDefault(x => x.Id == id);
            return View(tag);
        }
        [HttpPost]
        public IActionResult Edit(Tag tag)
        {
            var findTag = _context.Tags.FirstOrDefault(x => x.Name == tag.Name);
            if (findTag != null)
            {
                ViewBag.TagExist = "This tag is exist";
                return View(findTag);
            }
            _context.Tags.Update(tag);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var tag = _context.Tags.FirstOrDefault(x => x.Id == id);
            return View(tag);
        }
        [HttpPost]
        public IActionResult Delete(Tag tag)
        {
            _context.Tags.Remove(tag);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
