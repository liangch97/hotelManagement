namespace HotelManagement.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int ReservationId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty; // Cash, Card, Transfer
        public string Status { get; set; } = "Pending"; // Pending, Completed, Refunded
        public string? TransactionId { get; set; }
    }
}
