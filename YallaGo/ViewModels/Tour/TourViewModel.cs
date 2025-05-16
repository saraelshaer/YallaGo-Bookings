using System.ComponentModel.DataAnnotations;

namespace YallaGo.UI.ViewModels.Tour
{
    public class TourViewModel
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int Duration { get; set; }

        public int AvailableSeats { get; set; }
        public int DestinationId { get; set; }

    }
}
