using Microsoft.AspNetCore.Mvc;

namespace FiorEllo.Controllers
{
    public class FaqPageController:Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
