using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YallaGo.BLL.DTOs.TourDtos;

namespace YallaGo.BLL.Interfaces
{
    public interface ITourService
    {
        Task<IEnumerable<ReadTourDto>> GetAllToursAsync();
        Task<ReadTourDto> GetTourByIdAsync(int id);
        Task<ReadTourDto> CreateTourAsync(TourDto tourDto);
        Task<ReadTourDto> UpdateTourAsync(int id, TourDto tourDto);
        Task<bool> DeleteTourAsync(int id);
        Task<IEnumerable<ReadTourDto>> GetToursByDestinationIdAsync(int destinationId);
        Task<IEnumerable<ReadTourDto>> GetToursByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<ReadTourDto>> SearchToursAsync(string searchTerm);
    }
}
