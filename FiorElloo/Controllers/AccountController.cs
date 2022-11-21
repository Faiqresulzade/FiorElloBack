using FiorEllo.Models;
using FiorEllo.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FiorEllo.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public async Task<IActionResult> Register()

        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(AccountRegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,

            };
            var result = await _userManager.CreateAsync(user, model.PassWord);
            if (result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
            return RedirectToAction("login");
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "UserName or Password is InCorrect!!");
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(user, model.PassWord, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "UserName or Password is InCorrect!!");
                return View(model);
            }

            if(!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }
            else
            {
                return RedirectToAction("index", "home");

            }
            
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login");
        }
    }
}
