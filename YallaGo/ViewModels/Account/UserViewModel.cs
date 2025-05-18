using System.ComponentModel.DataAnnotations;

namespace YallaGo.UI.ViewModels.Account
{
    public class UserViewModel : RegisterUserViewModel
    {
        [Required]
        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}
