using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YallaGo.UI.ViewModels.Roles;

namespace YallaGo.UI.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateRole(RoleViewModel model )
        {
            if (ModelState.IsValid) 
            {
                var role = new IdentityRole
                {
                    Name = model.RoleName
                };
                var result = _roleManager.CreateAsync(role).Result;
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
    }
}
