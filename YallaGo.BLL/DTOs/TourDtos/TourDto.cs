namespace YallaGo.BLL.DTOs.TourDtos
{
    public class TourDto
    {
        public string Name { get; set; }

        public decimal Price { get; set; }
        public string Description { get; set; }

        public int Duration { get; set; }

        public int AvailableSeats { get; set; }
        public int DestinationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ImageURL { get; set; }
    }
}
