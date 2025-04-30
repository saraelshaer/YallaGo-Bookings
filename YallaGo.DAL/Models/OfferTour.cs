namespace YallaGo.DAL.Models
{
    public class OfferTour
    {
        public int TourId { get; set; }

        public Tour Tour { get; set; }

        public int OfferId { get; set; }

        public Offer Offer { get; set; }
    }
}
