using Microsoft.AspNetCore.Mvc;
using YallaGo.BLL.DTOs.DestinationDtos;
using YallaGo.BLL.Interfaces;
using YallaGo.UI.Helpers;
using YallaGo.UI.ViewModels.Destination;

namespace YallaGo.UI.Controllers
{
    public class DestinationController : Controller
    {
        private readonly IDestinationService _destinationService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DestinationController(IDestinationService destinationService, IWebHostEnvironment webHostEnvironment)
        {
            _destinationService = destinationService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var destinations = await _destinationService.GetAllDestinationsAsync();
            return View(destinations);
        }

        public async Task<IActionResult> AdminIndex()
        {
            var destinations = await _destinationService.GetAllDestinationsAsync();
            return View(destinations);
        }

        public async Task<IActionResult> Details(int id)
        {
            var destination = await _destinationService.GetDestinationByIdAsync(id);
            if (destination == null)
            {
                return NotFound();
            }
            return View(destination);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDestinationViewModel destinationVM)
        {
            if (ModelState.IsValid)
            {
                var destinationDto = new DestinationDto
                {
                    Name = destinationVM.Name,
                    Description = destinationVM.Description,
                    Country = destinationVM.Country,
                };
                if (destinationVM.ImageFile != null)
                {
                    destinationDto.ImageUrl = ImageHelper.SaveImage(destinationVM.ImageFile, "images", _webHostEnvironment);
                }

                await _destinationService.CreateDestinationAsync(destinationDto);

                return RedirectToAction("Index");

            }
            return View(destinationVM);

        }

        public async Task<IActionResult> Edit(int id)
        {
            var destination = await _destinationService.GetDestinationByIdAsync(id);
            if (destination == null)
            {
                return NotFound();
            }
            var destinationVM = new UpdatedestinationVM
            {
                Name = destination.Name,
                Description = destination.Description,
                Country = destination.Country,
                ImageUrl = destination.ImageUrl
            };
            return View(destinationVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdatedestinationVM destinationVM)
        {
            if (ModelState.IsValid)
            {
                var destinationDto = new DestinationDto
                {
                    Name = destinationVM.Name,
                    Description = destinationVM.Description,
                    Country = destinationVM.Country,
                    ImageUrl = destinationVM.ImageUrl
                };
                if (destinationVM.ImageFile != null)
                {
                    destinationDto.ImageUrl = ImageHelper.SaveImage(destinationVM.ImageFile, "images", _webHostEnvironment);
                }
                var updatedDestination = await _destinationService.UpdateDestinationAsync(id, destinationDto);
                if (updatedDestination == null)
                {
                    return NotFound();
                }
                return RedirectToAction("Index");
            }
            return View(destinationVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _destinationService.DeleteDestinationAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return RedirectToAction("AdminIndex");
        }
    }
}
