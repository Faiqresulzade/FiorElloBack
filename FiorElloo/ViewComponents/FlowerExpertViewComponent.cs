using front_to_back.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorEllo.ViewComponents
{
    public class FlowerExpertViewComponent: ViewComponent
    {
        private readonly AppDbContext _appDbContext;

        public FlowerExpertViewComponent(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var FlowerExpert = await _appDbContext.FlowerExperts.ToListAsync();
            return View(FlowerExpert);
        }
    }
}
