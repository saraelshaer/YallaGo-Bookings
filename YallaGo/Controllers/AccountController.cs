using MedPoint.UI.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using YallaGo.DAL.Models;

namespace YallaGo.UI.Controllers
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
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel registerUserVM)
        {
            if (ModelState.IsValid)
            {
                // Registration logic here
                // For example, create a new user and save to the database
                // Redirect to a success page or login page
                User user = new User
                {
                    FirstName = registerUserVM.FirstName,
                    LastName = registerUserVM.LastName,
                    Email = registerUserVM.Email,
                    UserName = new MailAddress(registerUserVM.Email).User
                };

                IdentityResult result = await _userManager.CreateAsync(user, registerUserVM.Password);
                if (result.Succeeded)
                {
                    // Optionally, you can sign in the user after registration
                    await _userManager.AddToRoleAsync(user, "User");
                    // Sign in the user (create cookie)
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(registerUserVM);
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
