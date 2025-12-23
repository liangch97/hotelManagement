using HotelManagement.Models;
using HotelManagement.Data;

namespace HotelManagement.Services
{
    public class HotelService
    {
        private readonly RoomRepository _roomRepository;
        private readonly GuestRepository _guestRepository;
        private readonly ReservationRepository _reservationRepository;

        public HotelService(DatabaseConnection dbConnection)
        {
            _roomRepository = new RoomRepository(dbConnection);
            _guestRepository = new GuestRepository(dbConnection);
            _reservationRepository = new ReservationRepository(dbConnection);
        }

        // Room Management
        public List<Room> GetAllRooms() => _roomRepository.GetAllRooms();
        public List<Room> GetAvailableRooms() => _roomRepository.GetAvailableRooms();
        public Room? GetRoomByNumber(string roomNumber) => _roomRepository.GetRoomByNumber(roomNumber);
        public bool AddRoom(Room room) => _roomRepository.AddRoom(room);
        public bool UpdateRoom(Room room) => _roomRepository.UpdateRoom(room);
        public bool DeleteRoom(int roomId) => _roomRepository.DeleteRoom(roomId);

        // Guest Management
        public List<Guest> GetAllGuests() => _guestRepository.GetAllGuests();
        public Guest? GetGuestById(int guestId) => _guestRepository.GetGuestById(guestId);
        public Guest? GetGuestByIdNumber(string idNumber) => _guestRepository.GetGuestByIdNumber(idNumber);
        public int RegisterGuest(Guest guest)
        {
            guest.RegistrationDate = DateTime.Now;
            return _guestRepository.AddGuest(guest);
        }
        public bool UpdateGuest(Guest guest) => _guestRepository.UpdateGuest(guest);

        // Reservation Management
        public List<Reservation> GetAllReservations() => _reservationRepository.GetAllReservations();
        public Reservation? GetReservationById(int reservationId) => _reservationRepository.GetReservationById(reservationId);
        public List<Reservation> GetReservationsByGuest(int guestId) => _reservationRepository.GetReservationsByGuest(guestId);

        public int MakeReservation(int guestId, int roomId, DateTime checkInDate, DateTime checkOutDate, string? specialRequests = null)
        {
            var room = _roomRepository.GetRoomById(roomId);
            if (room == null || room.Status != "Available")
            {
                throw new InvalidOperationException("Room is not available for reservation.");
            }

            int numberOfNights = (checkOutDate - checkInDate).Days;
            decimal totalAmount = numberOfNights * room.PricePerNight;

            var reservation = new Reservation
            {
                GuestId = guestId,
                RoomId = roomId,
                CheckInDate = checkInDate,
                CheckOutDate = checkOutDate,
                Status = "Confirmed",
                TotalAmount = totalAmount,
                ReservationDate = DateTime.Now,
                SpecialRequests = specialRequests
            };

            int reservationId = _reservationRepository.AddReservation(reservation);
            if (reservationId > 0)
            {
                _roomRepository.UpdateRoomStatus(roomId, "Occupied");
            }

            return reservationId;
        }

        public bool CheckIn(int reservationId)
        {
            var reservation = _reservationRepository.GetReservationById(reservationId);
            if (reservation == null)
            {
                return false;
            }

            return _reservationRepository.UpdateReservationStatus(reservationId, "CheckedIn");
        }

        public bool CheckOut(int reservationId)
        {
            var reservation = _reservationRepository.GetReservationById(reservationId);
            if (reservation == null)
            {
                return false;
            }

            bool statusUpdated = _reservationRepository.UpdateReservationStatus(reservationId, "CheckedOut");
            if (statusUpdated)
            {
                _roomRepository.UpdateRoomStatus(reservation.RoomId, "Available");
            }

            return statusUpdated;
        }

        public bool CancelReservation(int reservationId)
        {
            var reservation = _reservationRepository.GetReservationById(reservationId);
            if (reservation == null)
            {
                return false;
            }

            bool cancelled = _reservationRepository.UpdateReservationStatus(reservationId, "Cancelled");
            if (cancelled && reservation.Status != "CheckedOut")
            {
                _roomRepository.UpdateRoomStatus(reservation.RoomId, "Available");
            }

            return cancelled;
        }

        // Reports
        public void DisplayRoomStatus()
        {
            var rooms = GetAllRooms();
            Console.WriteLine("\n===== Room Status Report =====");
            Console.WriteLine($"{"Room#",-8} {"Type",-15} {"Floor",-7} {"Price",-10} {"Capacity",-10} {"Status",-12}");
            Console.WriteLine(new string('-', 70));

            foreach (var room in rooms)
            {
                Console.WriteLine($"{room.RoomNumber,-8} {room.RoomType,-15} {room.Floor,-7} ${room.PricePerNight,-9:F2} {room.MaxOccupancy,-10} {room.Status,-12}");
            }
        }

        public void DisplayGuestList()
        {
            var guests = GetAllGuests();
            Console.WriteLine("\n===== Guest List =====");
            Console.WriteLine($"{"ID",-5} {"Name",-30} {"ID Number",-15} {"Phone",-15}");
            Console.WriteLine(new string('-', 70));

            foreach (var guest in guests)
            {
                string fullName = $"{guest.FirstName} {guest.LastName}";
                Console.WriteLine($"{guest.GuestId,-5} {fullName,-30} {guest.IdNumber,-15} {guest.Phone,-15}");
            }
        }

        public void DisplayReservations()
        {
            var reservations = GetAllReservations();
            Console.WriteLine("\n===== Reservations =====");
            Console.WriteLine($"{"ID",-5} {"Guest ID",-9} {"Room ID",-8} {"Check-In",-12} {"Check-Out",-12} {"Amount",-10} {"Status",-12}");
            Console.WriteLine(new string('-', 80));

            foreach (var reservation in reservations)
            {
                Console.WriteLine($"{reservation.ReservationId,-5} {reservation.GuestId,-9} {reservation.RoomId,-8} {reservation.CheckInDate.ToShortDateString(),-12} {reservation.CheckOutDate.ToShortDateString(),-12} ${reservation.TotalAmount,-9:F2} {reservation.Status,-12}");
            }
        }
    }
}
