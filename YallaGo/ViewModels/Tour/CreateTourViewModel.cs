using System.ComponentModel.DataAnnotations;

namespace YallaGo.UI.ViewModels.Tour
{
    public class CreateTourViewModel: TourViewModel
    {
        [Required]
        public IFormFile ImageFile { get; set; }
    }
}
