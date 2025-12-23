namespace HotelManagement.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int GuestId { get; set; }
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, CheckedIn, CheckedOut, Cancelled
        public decimal TotalAmount { get; set; }
        public DateTime ReservationDate { get; set; }
        public string? SpecialRequests { get; set; }
    }
}
