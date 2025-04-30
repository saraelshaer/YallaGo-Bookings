namespace YallaGo.DAL.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

        public DateTime ReviewDate { get; set; }

        // Foreign Keys
        public int UserId { get; set; }

        public User User { get; set; }

        public int TourId { get; set; }

        public Tour Tour { get; set; }
    }
}
