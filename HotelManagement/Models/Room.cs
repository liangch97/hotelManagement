namespace HotelManagement.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
        public string Status { get; set; } = "Available"; // Available, Occupied, Maintenance
        public int Floor { get; set; }
        public int MaxOccupancy { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
