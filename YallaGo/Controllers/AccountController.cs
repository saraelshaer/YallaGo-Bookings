using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Text.RegularExpressions;
using YallaGo.DAL.Models;
using YallaGo.UI.Helpers;
using YallaGo.UI.ViewModels.Account;

namespace YallaGo.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
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
                    
                    await _userManager.AddToRoleAsync(user, "User");
                    // Sign in the user (create cookie)
                    await _signInManager.SignInAsync(user, isPersistent: false); // ID, Name, Roles in cookie 

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
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel loginUserVM)
        {
            if(ModelState.IsValid)
            {
                // check in db
                var user = await _userManager.FindByEmailAsync(loginUserVM.Email);
                if (user != null)
                {
                    var found = await _userManager.CheckPasswordAsync(user, loginUserVM.Password);
                    if (found)
                    {
                        // create cookie
                        await _signInManager.SignInAsync(user, loginUserVM.RememberMe);

                        if(await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "Owner"))
                        {
                            return RedirectToAction("Dashboard", "Home");
                        }
                     
                        return RedirectToAction("Index", "Home");
                    }
                    
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(loginUserVM);
                
            }
            return View(loginUserVM);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }


        public async Task<JsonResult> CheckEmail(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                return Json($"Email {Email} is already in use.");
            }
            return Json(true);
        }

        public JsonResult CheckPassword(string Password)
        {
            if (Password.Length < 8)
            {
                return Json("Password must be at least 8 characters long.");
            }
            else if (!Regex.IsMatch(Password, @"[A-Z]"))
            {
                return Json("Password must contain at least one uppercase letter.");
            }
            else if (!Regex.IsMatch(Password, @"[a-z]"))
            {
                return Json("Password must contain at least one lowercase letter.");
            }
            else if (!Regex.IsMatch(Password, @"[0-9]"))
            {
                return Json("Password must contain at least one digit.");
            }
            else if (!Regex.IsMatch(Password, @"[_!@#$%?]"))
            {
                return Json("Password must contain at least one special character ( _ ,!, @, #, $, %, ?).");
            }
            return Json(true);
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            
            var user = await _userManager.GetUserAsync(User);
            var model = new UserProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ImageFileName = user.ImageURL,
            };
          

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(UserProfileViewModel model)
        {
            if (ModelState.IsValid) 
            {
                var user = _userManager.GetUserAsync(User).Result;
                // Update user properties
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserName = model.UserName;
                user.PhoneNumber = model.PhoneNumber;

                var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
                if (userWithSameEmail != null && userWithSameEmail.Id != user.Id)
                {
                    ModelState.AddModelError("Email", "This email is already in use.");
                    return View(model);
                }
                else
                {
                    user.Email = model.Email;
                }
                if(model.ImageFile != null)
                {
                    
                    user.ImageURL = ImageHelper.SaveImage(model.ImageFile,"usersImages", _webHostEnvironment);
                }
                // Save changes to the database
                var result = _userManager.UpdateAsync(user).Result;
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    TempData["Message"] = "Profile updated successfully.";
                    return RedirectToAction("Manage");
                }

            }


            return View(model);
        }


    }
}
