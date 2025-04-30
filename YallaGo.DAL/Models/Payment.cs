namespace YallaGo.DAL.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public DateTime PaymentDate { get; set; }

        public string PaymentMethod { get; set; }

        public decimal Amount { get; set; }

        // Foreign Key
        public int BookingId { get; set; }

        public Booking Booking { get; set; }
    }
}
