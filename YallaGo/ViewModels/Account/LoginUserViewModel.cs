using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace YallaGo.UI.ViewModels.Account
{
    public class LoginUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
