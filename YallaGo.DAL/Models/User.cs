using Microsoft.AspNetCore.Identity;

namespace YallaGo.DAL.Models
{
    public class User: IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? ImageURL { get; set; } = "defaultUserImage.png";

        // Navigation Properties
        public ICollection<Booking> Bookings { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
