using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YallaGo.BLL.Interfaces;
using YallaGo.Models;

namespace YallaGo.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDestinationService _destinationService;

        public HomeController(ILogger<HomeController> logger, IDestinationService destinationService)
        {
            _logger = logger;
            _destinationService = destinationService;
        }

        public async Task<IActionResult> Index()
        {
            var destinations = await _destinationService.GetAllDestinationsAsync();
            
            return View(destinations);
        }
        [Authorize(Roles = "owner,Admin")]
        public IActionResult Dashboard()
        {
            //ViewBag.NoOfCustomers = _unitOfWork.UserRepository.Count(u => u.UserRoles.Any(r => r.Role.Name.ToLower() != "admin") && u.IsActive);
            //ViewBag.NoOfOrders = _unitOfWork.OrderRepo.Count(o => o.User.IsActive);
            //ViewBag.TotalSales = _unitOfWork.OrderRepo.Sum(o => o.TotalAmount, o => o.PaymentStatus == Enums.PaymentStatus.Completed);
            //ViewBag.PendingOrders = _unitOfWork.OrderRepo.Count(o => o.OrderStatus == Enums.OrderStatus.Pending && o.User.IsActive);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
