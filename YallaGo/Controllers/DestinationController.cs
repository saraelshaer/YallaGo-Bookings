﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YallaGo.BLL.DTOs.DestinationDtos;
using YallaGo.BLL.Interfaces;
using YallaGo.UI.Helpers;
using YallaGo.UI.ViewModels.Destination;

namespace YallaGo.UI.Controllers
{
    [Authorize]
    public class DestinationController : Controller
    {
        private readonly IDestinationService _destinationService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DestinationController(IDestinationService destinationService, IWebHostEnvironment webHostEnvironment)
        {
            _destinationService = destinationService;
            _webHostEnvironment = webHostEnvironment;
        }
        [Authorize(Roles = "user,Admin,owner")]
        public async Task<IActionResult> Index()
        {
            var destinations = await _destinationService.GetAllDestinationsAsync();
            return View(destinations);
        }
        [Authorize(Roles = "owner,Admin")]
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
        [Authorize(Roles = "owner,Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "owner,Admin")]
        public async Task<IActionResult> Create(CreateDestinationViewModel destinationVM)
        {
            if (ModelState.IsValid)
            {
                var destinationDto = new DestinationDto
                {
                    Name = destinationVM.Name,
                    Country = destinationVM.Country,
                };
                if (destinationVM.ImageFile != null)
                {
                    destinationDto.ImageUrl = ImageHelper.SaveImage(destinationVM.ImageFile, "images/Destinations", _webHostEnvironment);
                }

                await _destinationService.CreateDestinationAsync(destinationDto);

                return RedirectToAction("AdminIndex");

            }
            return View(destinationVM);

        }
        [Authorize(Roles = "owner,Admin")]

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
                Country = destination.Country,
                ImageUrl = destination.ImageUrl
            };
            return View(destinationVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "owner,Admin")]
        public async Task<IActionResult> Edit(int id, UpdatedestinationVM destinationVM)
        {
            if (ModelState.IsValid)
            {
                var destinationDto = new DestinationDto
                {
                    Name = destinationVM.Name,
                    Country = destinationVM.Country,
                    ImageUrl = destinationVM.ImageUrl
                };
                if (destinationVM.ImageFile != null)
                {
                    destinationDto.ImageUrl = ImageHelper.SaveImage(destinationVM.ImageFile, "images/Destinations", _webHostEnvironment);
                }
                var updatedDestination = await _destinationService.UpdateDestinationAsync(id, destinationDto);
                if (updatedDestination == null)
                {
                    return NotFound();
                }
                return RedirectToAction("AdminIndex");
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
