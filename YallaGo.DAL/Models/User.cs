namespace YallaGo.DAL.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNo { get; set; }

        public string Password { get; set; }

        public string ImageURL { get; set; }

        // Navigation Properties
        public ICollection<Booking> Bookings { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
