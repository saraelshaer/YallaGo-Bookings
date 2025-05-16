namespace YallaGo.UI.ViewModels.Tour
{
    public class UpdateTourViewModel:TourViewModel
    {
        public string? ImageURL { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
