using Microsoft.AspNetCore.Mvc;

namespace YallaGo.UI.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
