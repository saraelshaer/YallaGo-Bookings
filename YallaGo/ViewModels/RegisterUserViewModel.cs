using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace YallaGo.UI.ViewModels
{
    public class RegisterUserViewModel
    {
        [MaxLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [MaxLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Remote(action: "CheckEmail" ,controller: "Account")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Remote(action: "CheckPassword", controller: "Account")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
