namespace YallaGo.DAL.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public int NumberOfPeople { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime BookingDate { get; set; }

        public string Status { get; set; }

        // Foreign Keys
        public int UserId { get; set; }

        public User User { get; set; }

        public int TourId { get; set; }

        public Tour Tour { get; set; }

        // Navigation
        public ICollection<Payment> Payments { get; set; }
    }
}
