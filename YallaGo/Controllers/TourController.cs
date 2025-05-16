using Microsoft.AspNetCore.Mvc;
using YallaGo.BLL.DTOs.TourDtos;
using YallaGo.BLL.Interfaces;
using YallaGo.UI.Helpers;
using YallaGo.UI.ViewModels.Tour;

namespace YallaGo.UI.Controllers
{
    public class TourController : Controller
    {
        private readonly ITourService _tourService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TourController(ITourService tourService, IWebHostEnvironment webHostEnvironment)
        {
            _tourService = tourService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> UserIndex()
        {
           var tours = await _tourService.GetAllToursAsync();
            return View(tours);
        }

        public async Task<IActionResult> AdminIndex()
        {
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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                };
                if(tourVM.ImageFile != null)
                {
                    var imagePath = ImageHelper.SaveImage(tourVM.ImageFile, "images/tours", _webHostEnvironment);
                    tourDto.ImageURL = imagePath;
                }

                var createdTour = await _tourService.CreateTourAsync(tourDto);
                return RedirectToAction("AdminIndex");
            }
            return View(tourVM);
        }

        [HttpGet]
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
                ImageURL = tour.ImageURL
            };
            return View(tourVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                };
                if (tourVM.ImageFile != null)
                {
                    var imagePath = ImageHelper.SaveImage(tourVM.ImageFile, "images/tours", _webHostEnvironment);
                    tourDto.ImageURL = imagePath;
                }
                await _tourService.UpdateTourAsync(id, tourDto);

                return RedirectToAction("AdminIndex");
            }
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
