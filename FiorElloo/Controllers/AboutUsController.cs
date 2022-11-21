using front_to_back.DAL;
using Microsoft.AspNetCore.Mvc;

namespace FiorEllo.Controllers
{
    public class AboutUsController: Controller
    {
        private readonly AppDbContext _appDbContext;

        public AboutUsController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
