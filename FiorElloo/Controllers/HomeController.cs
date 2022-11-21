using FiorEllo.ViewModels.Home;
using front_to_back.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorEllo.Controllers
{
    public class HomeController:Controller
    {
        private readonly AppDbContext _appDbContext;

        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var model = new HomeIndexViewModel
            {
                FlowerExperts = await _appDbContext.FlowerExperts.ToListAsync(),
                Products=await _appDbContext.Products.Take(4)
                .ToListAsync(),
            };
            return View(model);
        }
    }
}
