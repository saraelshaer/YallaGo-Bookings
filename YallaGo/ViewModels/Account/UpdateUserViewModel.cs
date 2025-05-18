using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace YallaGo.UI.ViewModels.Account
{
    public class UpdateUserViewModel
    {
        [MaxLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [MaxLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        public IEnumerable<string> Roles { get; set; } = new List<string>();

        [Display(Name = "Profile Image")]
        public IFormFile? ImageFile { get; set; }

        public string? ImageUrl { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }

    }
}
