using FiorEllo.Models;
using FiorEllo.ViewModels.Blog;
using front_to_back.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorEllo.Controllers
{
    public class BlogController:Controller
    {
        private readonly AppDbContext _appDbContext;

        public BlogController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index(BlogIndexViewModel model)
        {
            var blogs =await PaginateBlogsAsync(model.Page, model.Take);
            model = new BlogIndexViewModel
            {
                BlogMasonries = blogs,
                PageCount =await GetPageCountAsync(model.Take),
                Page=model.Page,
            };
            return View(model);
        }

        private async Task<List<BlogMasonry>> PaginateBlogsAsync(int page, int take)
        {
            return await _appDbContext.BlogMasonries.OrderByDescending(b => b.Id)
                 .Skip(page * take - take)
                 .Take(take).ToListAsync();
        }
         
        private async Task<int> GetPageCountAsync(int take)
        {
           var blogCount= await _appDbContext.BlogMasonries.CountAsync();
            return (int)Math.Ceiling((decimal)blogCount / take);
        }

        public async Task<IActionResult> Details()
        {
            return View();
        }
    }
}
