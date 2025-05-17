using YallaGo.BLL.DTOs.TourDtos;
using YallaGo.BLL.Interfaces;
using YallaGo.DAL.Models;
using YallaGo.DAL.Repositories;

namespace YallaGo.BLL.Services
{
    public class TourService : ITourService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TourService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ReadTourDto> CreateTourAsync(TourDto tourDto)
        {
            var tour = new Tour
            {
                Name = tourDto.Name,
                Description = tourDto.Description,
                Price = tourDto.Price,
                Duration = tourDto.Duration,
                DestinationId = tourDto.DestinationId,
                AvailableSeats = tourDto.AvailableSeats,
                ImageURL = tourDto.ImageURL
            };
            await _unitOfWork.TourRepo.AddAsync(tour);
            await _unitOfWork.CompleteAsync();

            return new ReadTourDto
            {
                Id = tour.Id,
                Name = tour.Name,
                Description = tour.Description,
                Price = tour.Price,
                Duration = tour.Duration,
                DestinationId = tour.DestinationId,
                AvailableSeats = tour.AvailableSeats,
                ImageURL = tour.ImageURL
            };
        }

        public async Task<bool> DeleteTourAsync(int id)
        {
            var tour = await _unitOfWork.TourRepo.GetByIdAsync(id);
            if (tour == null)
            {
                return false;
            }
            _unitOfWork.TourRepo.HardDelete(tour);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IEnumerable<ReadTourDto>> GetAllToursAsync()
        {
            var tours = await _unitOfWork.TourRepo.GetAllAsync(includes: new[] { "Destination" } , orderBy: t=> t.CreatedAt);
            var tourDtos = tours.Select(t => new ReadTourDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Price = t.Price,
                Duration = t.Duration,
                DestinationId = t.DestinationId,
                AvailableSeats = t.AvailableSeats,
                ImageURL = t.ImageURL,
                DestinationName = t.Destination.Name,
                StartDate = t.StartDate,
                CreatedAt = t.CreatedAt

            }).ToList();

            return tourDtos;
        }

        public async Task<ReadTourDto> GetTourByIdAsync(int id)
        {
            var tour = await _unitOfWork.TourRepo.FindAsync(t => t.Id == id, includes: new[] { "Destination" });
            if (tour == null)
            {
                return null;
            }
            var tourDto = new ReadTourDto
            {
                Id = tour.Id,
                Name = tour.Name,
                Description = tour.Description,
                Price = tour.Price,
                Duration = tour.Duration,
                DestinationId = tour.DestinationId,
                AvailableSeats = tour.AvailableSeats,
                ImageURL = tour.ImageURL,
                DestinationName = tour.Destination.Name,
                StartDate = tour.StartDate,
                CreatedAt = tour.CreatedAt

            };
            return tourDto;
        }

        public async Task<IEnumerable<ReadTourDto>> GetToursByDestinationIdAsync(int destinationId)
        {
            var tours = await _unitOfWork.TourRepo.GetAllAsync(t => t.DestinationId == destinationId , orderBy: t => t.CreatedAt);
            var tourDtos = tours.Select(t => new ReadTourDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Price = t.Price,
                Duration = t.Duration,
                DestinationId = t.DestinationId,
                DestinationName = t.Destination.Name,
                AvailableSeats = t.AvailableSeats,
                ImageURL = t.ImageURL,
                StartDate = t.StartDate,
                CreatedAt = t.CreatedAt
            }).ToList();

            return tourDtos;

        }

        public async Task<IEnumerable<ReadTourDto>> GetToursByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            var tours = await _unitOfWork.TourRepo.GetAllAsync(t => t.Price >= minPrice && t.Price <= maxPrice, orderBy: t => t.CreatedAt);
            var tourDtos = tours.Select(t => new ReadTourDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Price = t.Price,
                Duration = t.Duration,
                DestinationId = t.DestinationId,
                AvailableSeats = t.AvailableSeats,
                ImageURL = t.ImageURL,
                StartDate = t.StartDate,
            }).ToList();

            return tourDtos;
        }

        public async Task<IEnumerable<ReadTourDto>> SearchToursAsync(string searchTerm)
        {
            var tours = await _unitOfWork.TourRepo.GetAllAsync(t => t.Name.Contains(searchTerm));
            var tourDtos = tours.Select(t => new ReadTourDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Price = t.Price,
                Duration = t.Duration,
                DestinationId = t.DestinationId,
                AvailableSeats = t.AvailableSeats,
                ImageURL = t.ImageURL
            }).ToList();

            return tourDtos;
        }

        public async Task<ReadTourDto> UpdateTourAsync(int id, TourDto tourDto)
        {
            var tour = await _unitOfWork.TourRepo.GetByIdAsync(id);
            if (tour == null)
            {
                return null;
            }
            tour.Name = tourDto.Name;
            tour.Description = tourDto.Description;
            tour.Price = tourDto.Price;
            tour.Duration = tourDto.Duration;
            tour.DestinationId = tourDto.DestinationId;
            tour.AvailableSeats = tourDto.AvailableSeats;
            tour.ImageURL = tourDto.ImageURL;
            tour.StartDate = tourDto.StartDate;
            tour.CreatedAt = DateTime.Now;

            await _unitOfWork.CompleteAsync();

            return new ReadTourDto
            {
                Id = tour.Id,
                Name = tour.Name,
                Description = tour.Description,
                Price = tour.Price,
                Duration = tour.Duration,
                DestinationId = tour.DestinationId,
                AvailableSeats = tour.AvailableSeats,
                ImageURL = tour.ImageURL
            };
        }
    }
}
