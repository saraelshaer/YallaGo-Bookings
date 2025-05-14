using System.ComponentModel.DataAnnotations;

namespace YallaGo.UI.ViewModels
{
    public class RoleViewModel
    {
        [Required]
        public string RoleName { get; set; }

    }
}
