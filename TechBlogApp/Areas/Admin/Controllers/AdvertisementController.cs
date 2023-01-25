using Microsoft.AspNetCore.Mvc;
using TechBlogApp.Data;
using TechBlogApp.Models;
using TechBlogApp.Services;

namespace TechBlogApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdvertisementController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _web;

        public AdvertisementController(AppDbContext context, IWebHostEnvironment web)
        {
            _context = context;
            _web = web;
        }

        public IActionResult Index()
        {
           var adv= _context.Advertisements.ToList();
            return View(adv);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Advertisement advertisement, IFormFile Photo)
        {
            var path = "/uploads/" + Guid.NewGuid() + Photo.FileName;
            using (var fileStream = new FileStream(_web.WebRootPath + path, FileMode.Create))
            {
                Photo.CopyTo(fileStream);
            }
            Advertisement ads = new()
            {
                Name = advertisement.Name,
                Price=advertisement.Price,
                Rate=advertisement.Rate,
                SizeX=advertisement.SizeX,
                SizeY=advertisement.SizeY,
                DirectionAddress= "https://"+ advertisement.DirectionAddress,
                CreatedDate =DateTime.Now,
                Click=0,
                View=0,
                PhotoUrl=path
            };

            _context.Advertisements.Add(ads);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Detail(int id)
        {
            var adv = _context.Advertisements.FirstOrDefault(x => x.Id == id);
            return View(adv);
        }
       
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var adv = _context.Advertisements.FirstOrDefault(x => x.Id == id);
            return View(adv);
        }
        [HttpPost]
        public IActionResult Delete(Advertisement advertisement)
        {
            var result = _context.Advertisements.FirstOrDefault(x => x.Id == advertisement.Id);
            advertisement.IsDeleted = true;
            _context.Advertisements.Update(result);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
