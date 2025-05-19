using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YallaGo.BLL.DTOs.TourDtos;
using YallaGo.BLL.Interfaces;
using YallaGo.UI.Helpers;
using YallaGo.UI.ViewModels.Tour;

namespace YallaGo.UI.Controllers
{
    [Authorize]
    public class TourController : Controller
    {
        private readonly ITourService _tourService;
        private readonly IDestinationService _destinationService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TourController(ITourService tourService, IDestinationService destinationService, IWebHostEnvironment webHostEnvironment)
        {
            _tourService = tourService;
            _destinationService = destinationService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> UserIndex()
        {
           var tours = await _tourService.GetAllToursAsync();
            return View(tours);
        }
        [Authorize(Roles = "owner,Admin")]
        public async Task<IActionResult> AdminIndex()
        {
            ViewBag.Destinations = await _destinationService.GetAllDestinationsAsync();
            var tours = await _tourService.GetAllToursAsync();
            return View(tours);
        }

        public async Task<IActionResult> Details(int id)
        {
            var tour = await _tourService.GetTourByIdAsync(id);
            if (tour == null)
            {
                return NotFound();
            }
            return View(tour);
        }
        public async Task<IActionResult> Search(string searchTerm)
        {
            var tours = await _tourService.SearchToursAsync(searchTerm);
            return View("UserIndex", tours);
        }

        public async Task<IActionResult> FilterByPrice(decimal minPrice, decimal maxPrice)
        {
            var tours = await _tourService.GetToursByPriceRangeAsync(minPrice, maxPrice);
            return View("UserIndex", tours);
        }

        public async Task<IActionResult> FilterByDestination(int destinationId)
        {
            var tours = await _tourService.GetToursByDestinationIdAsync(destinationId);
            return Json( tours);
        }

        [HttpGet]
        [Authorize(Roles = "owner,Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Destinations = await _destinationService.GetAllDestinationsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "owner,Admin")]
        public async Task<IActionResult> Create(CreateTourViewModel tourVM)
        {
            if (ModelState.IsValid)
            {
                var tourDto = new TourDto
                {
                    Name = tourVM.Name,
                    Description = tourVM.Description,
                    Price = tourVM.Price,
                    Duration = tourVM.Duration,
                    DestinationId = tourVM.DestinationId,
                    AvailableSeats = tourVM.AvailableSeats,
                    StartDate = tourVM.StartDate,
                };
                if(tourVM.ImageFile != null)
                {
                    var imagePath = ImageHelper.SaveImage(tourVM.ImageFile, "images/tours", _webHostEnvironment);
                    tourDto.ImageURL = imagePath;
                }

                var createdTour = await _tourService.CreateTourAsync(tourDto);
                return RedirectToAction("AdminIndex");
            }

            ViewBag.Destinations = await _destinationService.GetAllDestinationsAsync();
            return View(tourVM);
        }

        [HttpGet]
        [Authorize(Roles = "owner,Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var tour = await _tourService.GetTourByIdAsync(id);
            if (tour == null)
            {
                return NotFound();
            }
            var tourVM = new UpdateTourViewModel
            {
                Name = tour.Name,
                Description = tour.Description,
                Price = tour.Price,
                Duration = tour.Duration,
                DestinationId = tour.DestinationId,
                AvailableSeats = tour.AvailableSeats,
                ImageURL = tour.ImageURL,
                StartDate = tour.StartDate,
            };

            ViewBag.Destinations = await _destinationService.GetAllDestinationsAsync();
            return View(tourVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "owner,Admin")]
        public async Task<IActionResult> Edit(int id, UpdateTourViewModel tourVM)
        {
            if (ModelState.IsValid)
            {
                var tourDto = new TourDto
                {
                    Name = tourVM.Name,
                    Description = tourVM.Description,
                    Price = tourVM.Price,
                    Duration = tourVM.Duration,
                    DestinationId = tourVM.DestinationId,
                    AvailableSeats = tourVM.AvailableSeats,
                    ImageURL = tourVM.ImageURL,
                    StartDate = tourVM.StartDate,
                    CreatedAt = DateTime.Now,
                };
                if (tourVM.ImageFile != null)
                {
                    var imagePath = ImageHelper.SaveImage(tourVM.ImageFile, "images/tours", _webHostEnvironment);
                    tourDto.ImageURL = imagePath;
                }
                await _tourService.UpdateTourAsync(id, tourDto);

                return RedirectToAction("AdminIndex");
            }
            ViewBag.Destinations = await _destinationService.GetAllDestinationsAsync();
            return View(tourVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var IsDeleted = await _tourService.DeleteTourAsync(id);
            if (!IsDeleted)
            {
                return NotFound();
            }
            return RedirectToAction("AdminIndex");
        }
    }
}
