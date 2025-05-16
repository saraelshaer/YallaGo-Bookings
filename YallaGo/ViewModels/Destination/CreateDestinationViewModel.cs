using System.ComponentModel.DataAnnotations;

namespace YallaGo.UI.ViewModels.Destination
{
    public class CreateDestinationViewModel: DestinationViewModel
    {
        [Required(ErrorMessage = "Please select an image file.")]
        public IFormFile ImageFile { get; set; }
    }

}
