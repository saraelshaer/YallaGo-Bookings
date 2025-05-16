using System.ComponentModel.DataAnnotations;

namespace YallaGo.UI.ViewModels.Roles
{
    public class RoleViewModel
    {
        [Required]
        public string RoleName { get; set; }

    }
}
