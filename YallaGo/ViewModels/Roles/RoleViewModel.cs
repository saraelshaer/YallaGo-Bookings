using System.ComponentModel.DataAnnotations;

namespace YallaGo.UI.ViewModels.Roles
{
    public class RoleViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

    }
}
