using YallaGo.BLL.DTOs.DestinationDtos;
using YallaGo.BLL.Interfaces;
using YallaGo.DAL.Models;
using YallaGo.DAL.Repositories;
namespace YallaGo.BLL.Services
{
    public class DestinationService : IDestinationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DestinationService(IUnitOfWork unitOfWork )
        {
            _unitOfWork = unitOfWork;
        }
        public async Task CreateDestinationAsync(DestinationDto destinationDto)
        {
            var destination = new Destination
            {
                Name = destinationDto.Name,
                ImagePath = destinationDto.ImageUrl,
                Country = destinationDto.Country
            };

            await _unitOfWork.DestinationRepo.AddAsync(destination);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> DeleteDestinationAsync(int id)
        {
            var destination = await _unitOfWork.DestinationRepo.GetByIdAsync(id);
            if (destination == null)
            {
                return false;
            }
            _unitOfWork.DestinationRepo.HardDelete(destination);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IEnumerable<ReadDestinationDto>> GetAllDestinationsAsync()
        {
            var destinations = await _unitOfWork.DestinationRepo.GetAllAsync();
            return destinations.Select(d => new ReadDestinationDto
            {
                Id = d.Id,
                Name = d.Name,
                ImageUrl = d.ImagePath,
                Country = d.Country
            }).ToList();
        }

        public async Task<ReadDestinationDto> GetDestinationByIdAsync(int id)
        {
            var destination = await _unitOfWork.DestinationRepo.GetByIdAsync(id);
            if (destination == null)
            {
                return null;
            }
            return new ReadDestinationDto
            {
                Id = destination.Id,
                Name = destination.Name,
                ImageUrl = destination.ImagePath,
                Country = destination.Country
            };

        }

        public async Task<ReadDestinationDto> UpdateDestinationAsync(int id, DestinationDto destinationDto)
        {
            var destination = await _unitOfWork.DestinationRepo.GetByIdAsync(id);
            if (destination == null)
            {
                return null;
            }
            destination.Name = destinationDto.Name;
            destination.ImagePath = destinationDto.ImageUrl;
            destination.Country = destinationDto.Country;

            await _unitOfWork.CompleteAsync();
            return new ReadDestinationDto
            {
                Id = destination.Id,
                Name = destination.Name,
                ImageUrl = destination.ImagePath,
                Country = destination.Country
            };
        }
    }
}
