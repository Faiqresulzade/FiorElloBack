using FiorEllo.Areas.Admin.ViewModels.FlowerExpert;
using front_to_back.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorEllo.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class FlowerExpertController:Controller
	{
		private readonly AppDbContext _appDbContext;

		public FlowerExpertController(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}
		public async Task<IActionResult> Index()
		{
			var model = new FlowerExpertIndexViewModel
			{
				FlowerExperts = await _appDbContext.FlowerExperts.ToListAsync()
			};
			return View(model);
		}
	}
}
