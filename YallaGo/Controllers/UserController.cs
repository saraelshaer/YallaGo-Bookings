using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Security.Claims;
using YallaGo.DAL.Models;
using YallaGo.UI.Helpers;
using YallaGo.UI.ViewModels.Account;

namespace YallaGo.UI.Controllers
{
    [Authorize(Roles = "owner,Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            users = users.OrderByDescending(u => u.JoinedAt).ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult AddNew()
        {
            ViewBag.Roles = _roleManager.Roles.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNew(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = new MailAddress(model.Email).User
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    foreach (var role in model.Roles)
                    {
                        await _userManager.AddToRoleAsync(user, role);
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            ViewBag.Roles = _roleManager.Roles.ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var model = new UpdateUserViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName,
                    ImageUrl = user.ImageURL,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles.ToList()
                };
                ViewBag.Roles = _roleManager.Roles.ToList();
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UpdateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.PhoneNumber = model.PhoneNumber;

                    var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
                    if (userWithSameEmail != null && userWithSameEmail.Id != user.Id)
                    {
                        ModelState.AddModelError("Email", "This email is already in use.");
                        ViewBag.Roles = _roleManager.Roles.ToList();
                        return View(model);
                    }
                    else
                    {
                        user.Email = model.Email;
                    }

                    var userWithSameUserName = await _userManager.FindByNameAsync(model.UserName);
                    if (userWithSameUserName != null && userWithSameUserName.Id != user.Id)
                    {
                        ModelState.AddModelError("UserName", "This Username is already in use.");
                        ViewBag.Roles = _roleManager.Roles.ToList();
                        return View(model);

                    }
                    else
                    {
                        user.UserName = model.UserName;
                    }

                    if (model.ImageFile != null)
                    {

                        user.ImageURL = ImageHelper.SaveImage(model.ImageFile, "usersImages", _webHostEnvironment);

                    }

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var currentRoles = await _userManager.GetRolesAsync(user);
                        foreach (var role in currentRoles)
                        {
                            await _userManager.RemoveFromRoleAsync(user, role);
                        }
                        foreach (var role in model.Roles)
                        {
                            await _userManager.AddToRoleAsync(user, role);
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            ViewBag.Roles = _roleManager.Roles.ToList();
            return View(model);
        }

        public IActionResult Details(string id)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                return View(user);
            }
            return NotFound();
        }
        [HttpGet]
        public async Task<IActionResult> CanDeleteUser(string id)
        {
            var deletedUser = await _userManager.FindByIdAsync(id);
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _userManager.FindByIdAsync(currentUserId);

            if (deletedUser == null)
            {
                return Json(new { canDelete = false, message = "User not found." });
            }

            if (await _userManager.IsInRoleAsync(deletedUser, "owner"))
            {
                return Json(new { canDelete = false, message = "You cannot delete an owner user." });
            }

            if (await _userManager.IsInRoleAsync(deletedUser, "Admin") && !await _userManager.IsInRoleAsync(currentUser, "owner"))
            {
                return Json(new { canDelete = false, message = "You cannot delete an admin unless you're an owner." });
            }

            return Json(new { canDelete = true });
        }

        public async Task<IActionResult> Delete(string id)
        {
            var deletedUser = await _userManager.FindByIdAsync(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var loginUser = await _userManager.FindByIdAsync(userId);

            if (deletedUser != null)
            {
                if (await _userManager.IsInRoleAsync(deletedUser, "owner"))
                {
                    TempData["ErrorMessage"] = "You cannot delete an owner user.";
                    return RedirectToAction("Index");
                }

                if (await _userManager.IsInRoleAsync(deletedUser, "Admin") &&
                    !await _userManager.IsInRoleAsync(loginUser, "owner"))
                {
                    TempData["ErrorMessage"] = "You cannot delete an admin user.";
                    return RedirectToAction("Index");
                }


                var result = await _userManager.DeleteAsync(deletedUser);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return NotFound();
        }

    }
}
