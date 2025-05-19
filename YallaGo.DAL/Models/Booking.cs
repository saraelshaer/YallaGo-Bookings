using YallaGo.DAL.Consts;

namespace YallaGo.DAL.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public int NumberOfPeople { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime BookingDate { get; set; }

        public BookingStatus Status { get; set; }

        // Foreign Keys
        public string UserId { get; set; }

        public User User { get; set; }

        public int TourId { get; set; }

        public Tour Tour { get; set; }

        // Navigation

    }
}
