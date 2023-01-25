using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechBlogApp.Data;
using TechBlogApp.Models;
using TechBlogApp.Services;

namespace TechBlogApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _environment;

        public ArticleController(AppDbContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment environment)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
        }

        public IActionResult Index()
        {
            var articles = _context.Articles.Include(x=>x.User).Include(x=>x.Category).Include(x=>x.ArticleTags).ThenInclude(x=>x.Tag).Where(data=>data.IsDeleted==false).ToList();
            return View(articles);
        }

        public IActionResult Create()
        {
            var tags = _context.Tags.ToList();
            var categories = _context.Categories.ToList();
            ViewBag.Categories = new SelectList(categories,"Id","Name");
            ViewData["Tags"] = tags;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Article article,IFormFile NewPhoto, List<int> Tags)
        {  

            var userId=_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            article.UserId = userId;
            var tags = _context.Tags.ToList();
            var categories = _context.Categories.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewData["Tags"] = tags;

            var photo = ImageService.UploadImage(NewPhoto, _environment);
            var seo_url = SeoUrlService.SeoUrl(article.Title);

            article.CreatedDate = DateTime.Now;
            article.UpdateddDate = DateTime.Now;
            article.SeoUrl = seo_url;
            article.PhotoUrl = photo;
            article.Views = 0;


            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();
            List<ArticleTag> tagList = new();

            for (int i = 0; i < tagList.Count; i++)
            {
                ArticleTag articleTag = new()
                {
                    ArticleId = article.Id,
                    TagId = Tags[i]
                };
                tagList.Add(articleTag);
            }
            await _context.ArticleTags.AddRangeAsync(tagList);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var tags = _context.Tags.ToList();
            var categories = _context.Categories.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewData["Tags"] = tags;
            var article = _context.Articles.Include(x=>x.ArticleTags).FirstOrDefault(x => x.Id == id);
            return View(article);
        }
        [HttpPost]
        public IActionResult Edit(Article article,IFormFile Photo, List<int> Tags)
        {
            article.UpdateddDate = DateTime.Now;
            article.IsActive = false;
            article.SeoUrl = SeoUrlService.SeoUrl(article.Title);
            if(Photo !=null)
            {
                article.PhotoUrl = ImageService.UploadImage(Photo, _environment);
            }
            var oldTags = _context.ArticleTags.Where(x => x.ArticleId == article.Id).ToList();
            _context.ArticleTags.RemoveRange(oldTags);

            _context.SaveChanges();
            List<ArticleTag> tagList = new();

            for (int i = 0; i < Tags.Count; i++)
            {
                ArticleTag articleTag = new()
                {
                    ArticleId = article.Id,
                    TagId = Tags[i]
                };
                tagList.Add(articleTag);
            }

            _context.ArticleTags.AddRange(tagList);
            _context.Articles.Update(article);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var delete = _context.Articles.FirstOrDefault(x => x.Id == id);
            return View(delete);
        }

        [HttpPost]
        public IActionResult Delete(Article article)
        {
            _context.Articles.FirstOrDefault(x => x.Id == article.Id);
            article.IsDeleted = true;
            _context.Articles.Remove(article);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
