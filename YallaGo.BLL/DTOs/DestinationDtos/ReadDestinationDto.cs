using YallaGo.BLL.DTOs.TourDtos;

namespace YallaGo.BLL.DTOs.DestinationDtos
{
    public class ReadDestinationDto : DestinationDto
    {
        public int Id { get; set; }

        public IEnumerable<ReadTourDto> Tours { get; set; } = new List<ReadTourDto>();
    }
}
