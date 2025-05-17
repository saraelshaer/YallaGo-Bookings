using Microsoft.AspNetCore.Mvc;

namespace YallaGo.UI.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
