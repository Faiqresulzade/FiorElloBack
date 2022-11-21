using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiorEllo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class DashboardController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
