using System.ComponentModel.DataAnnotations;

namespace YallaGo.UI.ViewModels.Destination
{
    public class DestinationViewModel
    {
        [MaxLength(80)]
        public string Name { get; set; }

        [MaxLength(80)]
        public string Country { get; set; }
      
        
    }
}
