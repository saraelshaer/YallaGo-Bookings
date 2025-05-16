using YallaGo.BLL.DTOs.DestinationDtos;

namespace YallaGo.BLL.Interfaces
{
    public interface IDestinationService
    {
        Task<IEnumerable<ReadDestinationDto>> GetAllDestinationsAsync();
        Task<ReadDestinationDto> GetDestinationByIdAsync(int id);
        Task CreateDestinationAsync(DestinationDto destinationDto);
        Task<ReadDestinationDto> UpdateDestinationAsync(int id, DestinationDto destinationDto);
        Task<bool> DeleteDestinationAsync(int id);
    }
}
