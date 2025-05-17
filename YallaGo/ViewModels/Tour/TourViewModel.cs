using System.ComponentModel.DataAnnotations;

namespace YallaGo.UI.ViewModels.Tour
{
    public class TourViewModel
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int Duration { get; set; }

        [Display(Name = "Available Seats")]
        public int AvailableSeats { get; set; }

        [Display(Name = "Destination Name")]
        public int DestinationId { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date")]
        [Required]
        public DateTime StartDate { get; set; } = DateTime.Now;

    }
}
