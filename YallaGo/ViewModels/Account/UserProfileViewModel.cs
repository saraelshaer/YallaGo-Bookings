using System.ComponentModel.DataAnnotations;

namespace YallaGo.UI.ViewModels.Account
{
    public class UserProfileViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Profile Image")]
        public IFormFile? ImageFile { get; set; }

        public string? ImageFileName { get; set; } = "defaultUserImage.png";
    }
}
