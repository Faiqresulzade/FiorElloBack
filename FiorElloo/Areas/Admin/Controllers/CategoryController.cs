using FiorEllo.Areas.Admin.ViewModels.Category;
using FiorEllo.Models;
using front_to_back.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorEllo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController:Controller
    {
        private readonly AppDbContext _appDbContext;

        public CategoryController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var model = new CategoryIndexViewModel
            {
                Categories = await _appDbContext.Categories.ToListAsync(),
            };
            return View(model);
        }

        #region Create
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateViewModel model)
        {


            if(!ModelState.IsValid) return View(model);
            var isExist =await _appDbContext.Categories.AnyAsync(c => c.Title.ToUpper().Trim() == model.Title.ToUpper().Trim());
            if (isExist)
            {
                ModelState.AddModelError("Title", "Bun adda title var!!");
                return View(model);
            }
            var category = new Category
            {
                Title = model.Title,
            };
            await _appDbContext.Categories.AddAsync(category);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var category = await _appDbContext.Categories.FindAsync(id);
            if (category == null) return NotFound();

            var model = new CategoryUpdateViewModel
            {
                Title = category.Title
            };
              return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, CategoryUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            if (id != model.Id) return BadRequest();
            var dbcategory= await _appDbContext.Categories.FindAsync(id);
            if(dbcategory == null) return NotFound();
            bool IsExist=await _appDbContext.Categories.AnyAsync(c=>c.Title.ToLower().Trim()==model.Title.ToLower().Trim()
                                                                  && c.Id!=model.Id);
            if (IsExist)
            {
                ModelState.AddModelError("Title", "Bele title movcuddur!!");
                return View(model);
            }
            dbcategory.Title = model.Title;
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _appDbContext.Categories.FindAsync(id);
            if(category == null) return NotFound();
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Deletecategory(int id)
        {
            var category = await _appDbContext.Categories.FindAsync(id);
            if (category == null) return NotFound();
            _appDbContext.Categories.Remove(category);
           await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var category = await _appDbContext.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }
    }
}
