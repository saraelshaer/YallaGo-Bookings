using Microsoft.AspNetCore.Identity;

namespace YallaGo.DAL.Models
{
    public class User: IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? ImageURL { get; set; } = "defaultUserImage.png";

        // Navigation Properties
        public WishList WishList { get; set; } = new WishList();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        public ICollection<Review> Reviews { get; set; }= new List<Review>();
    }
}
