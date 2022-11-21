using Microsoft.AspNetCore.Mvc;

namespace FiorEllo.Controllers
{
    public class MyAcountController:Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
