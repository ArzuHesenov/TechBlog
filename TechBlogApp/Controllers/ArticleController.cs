using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechBlogApp.Data;
using TechBlogApp.Models;
using TechBlogApp.ViewModels;

namespace TechBlogApp.Controllers
{
    public class ArticleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public ArticleController(AppDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public IActionResult Detail(int? id, string seoUrl)
        {
            if (id.Value == null)
            {
                return NotFound();
            }
            var article = _context.Articles.Include(x => x.User).Include(x => x.Category).Include(x => x.ArticleTags).ThenInclude(x => x.Tag).Include(x=>x.Comments).ThenInclude(x=>x.User).FirstOrDefault(x => x.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            var cookie = _httpContext.HttpContext.Request.Cookies["Views"];
            string[] findCookie = { "" };
            if (cookie != null)
            {
                findCookie = cookie.Split("-").ToArray();
            }
            if (!findCookie.Contains(article.Id.ToString()))
            {
                Response.Cookies.Append("Views", $"{cookie}-{article.Id}",
                    new CookieOptions
                    {
                        Secure = true,
                        HttpOnly = true,
                        Expires = DateTime.Now.AddDays(1)
                    }
                    );

                article.Views += 1;
                _context.Articles.Update(article);
                _context.SaveChanges();
            }

            var suggestArticle = _context.Articles.Include(x => x.Category).Where(x => x.Id != article.Id && x.CategoryId==article.CategoryId).Take(2).ToList();
            var after = _context.Articles.FirstOrDefault(x => x.Id > id);
            var before = _context.Articles.FirstOrDefault(x => x.Id < id);

            DetailVM detailVM = new()
            {
                Suggestions=suggestArticle,
                Article = article,
                Before=before,
                After=after
            };
            return View(detailVM);
        }
        [HttpPost]
        public async Task<IActionResult> Detail(Comment comment)
        {
            var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Comment newComment = new()
            {
                CommentedDate = DateTime.Now,
                UserId = userId,
                ArticleId = comment.ArticleId,
                Content = comment.Content
            };
            var article = _context.Articles.FirstOrDefault(x => x.Id == comment.ArticleId);

            await _context.Comments.AddAsync(newComment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Detail), new { id = article.Id, article.SeoUrl });
        }
    }
}