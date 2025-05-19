namespace YallaGo.DAL.Models
{
    public class Tour
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public int Duration { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime StartDate { get; set; }

        public int AvailableSeats { get; set; }

        public string ImageURL { get; set; }

        // Foreign Key
        public int DestinationId { get; set; }

        public Destination Destination { get; set; }

        // Navigation


        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

       
    }
}
