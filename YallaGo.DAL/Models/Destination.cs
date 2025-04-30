namespace YallaGo.DAL.Models
{
    public class Destination
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string ImagePath { get; set; }

        // Navigation Property
        public ICollection<Tour> Tours { get; set; }
    }
}
