using FiorEllo.Areas.Admin.ViewModels.Product;
using FiorEllo.Models;
using front_to_back.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FiorEllo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(AppDbContext appDbContext,
            IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        #region Index
        public async Task<IActionResult> Index(ProductIndexViewModel model)
        {
            var products= FilterByTitle(model.Title);
            products= FilterByCategory(products,model.CategoryId);
            products = FilterByPrice(products, model.MaxPrice, model.MinPrice);
            products = FilterByQuantity(products, model.MaxQuantity, model.MinQuantity);
            products = FilterByCreateAt(products, model.CreateAtStart, model.CreateAtEnd);
            model = new ProductIndexViewModel()
            {
                Products = await products.Include(p=>p.Category).ToListAsync(),
                Categories=await _appDbContext.Categories.Select(p => new SelectListItem
                {
                    Value=p.Id.ToString(),
                    Text=p.Title
                })
                .ToListAsync()
            };
            return View(model);
        }
        #endregion
        #region Filter
        private IQueryable<Product> FilterByTitle(string title)
        {
            return _appDbContext.Products.Where(p => !string.IsNullOrEmpty(title) ? p.Title.Contains(title) : true);
        }

        private IQueryable<Product> FilterByPrice(IQueryable<Product> products, double? maxPrice, double? minPrice)
        {
            return products.Where(p => (minPrice != null ? p.Price >= minPrice : true)&& (maxPrice != null ? p.Price <= maxPrice : true));
        }

        private IQueryable<Product> FilterByCategory(IQueryable<Product> products,int? categoryId)
        {
          return  products.Where(p => categoryId != null ? p.CategoryId == categoryId : true);
        }
        private IQueryable<Product> FilterByQuantity(IQueryable<Product> products, int? maxQuantity, int? minQuantity)
        {
            return products.Where(p => (minQuantity != null ? p.Quantity >= minQuantity : true) && (maxQuantity != null ? p.Quantity <= maxQuantity : true));
        }

        private IQueryable<Product> FilterByCreateAt(IQueryable<Product> products, DateTime? createAtStart,DateTime? createTimeEnd)
        {
          return  products.Where(p=>(createAtStart!=null? p.CreateAt>=createAtStart:true)&& (createTimeEnd!=null? p.CreateAt<=createTimeEnd:true));
        }
        #endregion
        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            var model = new ProductCreateViewModel
            {
                Categories = await _appDbContext.Categories.Select(c => new SelectListItem
                {
                    Text = c.Title,
                    Value = c.Id.ToString()
                })
                .ToListAsync()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            model.Categories= await _appDbContext.Categories.Select(c => new SelectListItem
            {
                Text = c.Title,
                Value = c.Id.ToString()
            })
               .ToListAsync();
            if (!ModelState.IsValid) return View(model);
            var category = await _appDbContext.Categories.FindAsync(model.CategoryId);
            if (category == null)
            {
                ModelState.AddModelError("Id", "Bu category movcud deyil!!");
                return View(model);
            }
            bool IsExist = await _appDbContext.Products.AnyAsync(p => p.Title.ToLower().Trim() == model.Title.ToLower().Trim());
            if (IsExist)
            {
                ModelState.AddModelError("Title", "Bu adda product movcuddur!!");
                return View(model);
            }
            if (model.Photo != null)
            {
                if (!model.Photo.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("Photo", "Yuklenen sekil image formatinda olmalidir!!");
                    return View(model);
                }
                if (model.Photo.Length / 1024 > 90)
                {
                    ModelState.AddModelError("Photo", "sekiln olcusu 90kbdan boyukdur!!");
                    return View(model);
                }

            }
            var fileName = $"{Guid.NewGuid()}_{model.Photo.FileName}";
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", fileName);
            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                await model.Photo.CopyToAsync(fileStream);
            }
            var product = new Product
            {
                Title = model.Title,
                Description = model.Description,
                CategoryId = model.CategoryId,
                PhotoName = fileName,
                Dimensions = model.Dimensions,
                Price = model.Price,
                Quantity = model.Quantity,
                Weight = model.Weight,
            };
            await _appDbContext.Products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();

            foreach (var photo in model.Photos)
            {
                if (!photo.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("Photo", "Yuklenen sekil image formatinda olmalidir!!");
                    return View(model);
                }
                if (photo.Length / 1024 > 90)
                {
                    ModelState.AddModelError("Photo", "sekiln olcusu 90kbdan boyukdur!!");
                    return View(model);
                }
                var fileNamee = $"{Guid.NewGuid()}_{photo.FileName}";
                var pathh = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", fileNamee);
                using (FileStream fileStream = new FileStream(pathh, FileMode.Create, FileAccess.ReadWrite))
                {
                    await photo.CopyToAsync(fileStream);
                }
                var productphoto = new ProductPhoto
                {
                    ProductId = product.Id,
                    Name = fileNamee
                };
                await _appDbContext.ProductPhotos.AddAsync(productphoto);
                await _appDbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var category = await _appDbContext.Categories.Select(c => new SelectListItem
            {
                Text = c.Title,
                Value = c.Id.ToString()
            })
                .ToListAsync();
            var product = await _appDbContext.Products.FindAsync(id);
            if (product == null) return NotFound();
            var model = new ProductUpdateViewModel
            {
                Title = product.Title,
                Description = product.Description,
                CategoryId = product.CategoryId,
                Dimensions = product.Dimensions,
                Price = product.Price,
                PhotoName = product.PhotoName,
                Quantity = product.Quantity,
                Weight = product.Weight,
                Category = await _appDbContext.Categories.Select(p => new SelectListItem
                {
                    Text = p.Title,
                    Value = p.Id.ToString(),
                })
                .ToListAsync()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, ProductUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            if (id != model.Id) return BadRequest();
            var product = await _appDbContext.Products.FindAsync(id);
            if (product == null) return NotFound();
            bool IsExist = await _appDbContext.Products.AnyAsync(p =>
                           p.Title.ToLower().Trim() == model.Title.ToLower().Trim() &&
                             p.Id != id);
            if (IsExist)
            {
                ModelState.AddModelError("Title", "bu adda product movcuddur!!");
                return View(model);
            }
            if (model.Photo != null)
            {
                if (!model.Photo.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("Photo", "Yuklenen sekil image formatinda olmalidir!!");
                    return View(model);
                }
                if (model.Photo.Length / 1024 > 90)
                {
                    ModelState.AddModelError("Photo", "sekiln olcusu 90kbdan boyukdur!!");
                    return View(model);
                }
                if (System.IO.File.Exists(product.PhotoName))
                {
                    System.IO.File.Delete(product.PhotoName);
                }
                var fileName = $"{Guid.NewGuid()}_{model.Photo.FileName}";
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", fileName);
                using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                {
                    await model.Photo.CopyToAsync(fileStream);
                }
                product.PhotoName = fileName;
            }

            foreach (var photo in model.Photos)
            {
                if (photo != null)
                {
                    if (!photo.ContentType.Contains("image/"))
                    {
                        ModelState.AddModelError("Photo", "Yuklenen sekil image formatinda olmalidir!!");
                        return View(model);
                    }
                    if (photo.Length / 1024 > 90)
                    {
                        ModelState.AddModelError("Photo", "sekiln olcusu 90kbdan boyukdur!!");
                        return View(model);
                    }
                }
            }
            foreach (var photo in model.Photos)
            {
                var fileName = $"{Guid.NewGuid()}_{photo.FileName}";
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", fileName);
                using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                {
                    await photo.CopyToAsync(fileStream);
                }
                var productphoto = new ProductPhoto
                {
                    ProductId = product.Id,
                    Name = fileName
                };
                await _appDbContext.AddAsync(productphoto);
                await _appDbContext.SaveChangesAsync();
            }
            product.Weight = model.Weight;
            product.Price = model.Price;
            product.Description = model.Description;
            product.Dimensions = model.Dimensions;
            product.Title = model.Title;
            product.CategoryId = model.CategoryId;
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product=await _appDbContext.Products.Include(x=>x.Category).Include(p => p.ProductPhotos).FirstOrDefaultAsync(p => p.Id == id);
            if(product==null) return NotFound();
            return View(product);
        }
        #endregion
        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _appDbContext.Products.FindAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult>Deletee(int id)
        {
            var product = await _appDbContext.Products.FindAsync(id);
            if (product == null) return NotFound();
            if (System.IO.File.Exists(product.PhotoName))
            {
                System.IO.File.Delete(product.PhotoName);
            }
            _appDbContext.Products.Remove(product);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
