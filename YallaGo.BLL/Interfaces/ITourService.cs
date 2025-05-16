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
        Task<IEnumerable<TourDto>> GetAllToursAsync();
        Task<TourDto> GetTourByIdAsync(int id);
        Task<TourDto> CreateTourAsync(TourDto tourDto);
        Task<TourDto> UpdateTourAsync(int id, TourDto tourDto);
        Task<bool> DeleteTourAsync(int id);
        Task<List<TourDto>> GetToursByDestinationIdAsync(int destinationId);
        Task<List<TourDto>> GetToursByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<List<TourDto>> SearchToursAsync(string searchTerm);
    }
}
