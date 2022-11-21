using FiorEllo.Models;
using FiorEllo.ViewModels.Basket;
using front_to_back.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorEllo.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _appDbContext;

        public BasketController(UserManager<User> userManager, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)return Unauthorized();
            var basket = await _appDbContext.Baskets
                                            .Include(b=>b.BasketProducts)
                                            .ThenInclude(bp=>bp.Product)
                                            .FirstOrDefaultAsync(b => b.UserId == user.Id);
           if(basket == null)return View();
            var model = new BasketIndexViewModel();
            foreach (var dbBasketProduct in basket.BasketProducts)
            {
                var basketProduct = new BasketProductViewModel
                {
                    Id = dbBasketProduct.Id,
                    Title=dbBasketProduct.Product.Title,
                    PhotoName=dbBasketProduct.Product.PhotoName,
                    Quantity=dbBasketProduct.Quantity,
                    StockQuantity=dbBasketProduct.Product.Quantity,
                    Price=dbBasketProduct.Product.Price
                };
                model.BasketProducts.Add(basketProduct);
            }
            return View(model);
        }
        public async Task<IActionResult> Add(BasketAddViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(model);
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();
            var basket = await _appDbContext.Baskets.FirstOrDefaultAsync(b => b.UserId == user.Id);
            var product = await _appDbContext.Products.FindAsync(model.Id);
            if (product == null) return NotFound();
            if (basket == null)
            {
                basket = new Basket
                {
                    UserId = user.Id,
                };
                await _appDbContext.Baskets.AddAsync(basket);
                await _appDbContext.SaveChangesAsync();
            }
            var basketProduct = await _appDbContext.BasketProducts.FirstOrDefaultAsync(b => b.ProductId == model.Id);
            if (basketProduct != null)
            {
                basketProduct.Quantity++;
                //await _appDbContext.SaveChangesAsync();
            }
            else
            {
                basketProduct = new BasketProduct
                {
                    ProductId = product.Id,
                    BasketId = basket.Id,
                    Quantity = 1,
                };
                await _appDbContext.BasketProducts.AddAsync(basketProduct);
            }
            await _appDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
