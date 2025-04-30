namespace YallaGo.DAL.Models
{
    public class Offer
    {
        public int Id { get; set; }

        public decimal DiscountPercentage { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        // Navigation
        public ICollection<OfferTour> OfferTours { get; set; }
    }
}
