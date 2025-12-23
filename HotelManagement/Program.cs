using HotelManagement.Data;
using HotelManagement.Services;
using HotelManagement.Models;

namespace HotelManagement
{
    class Program
    {
        private static HotelService? _hotelService;
        private static DatabaseConnection? _dbConnection;

        static void Main(string[] args)
        {
            Console.WriteLine("===== Hotel Management System =====");
            Console.WriteLine("Database: MySQL");
            Console.WriteLine("====================================\n");

            // Initialize database connection
            if (!InitializeDatabase())
            {
                Console.WriteLine("Failed to initialize database. Press any key to exit...");
                Console.ReadKey();
                return;
            }

            // Main menu loop
            bool exit = false;
            while (!exit)
            {
                DisplayMainMenu();
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        RoomManagementMenu();
                        break;
                    case "2":
                        GuestManagementMenu();
                        break;
                    case "3":
                        ReservationManagementMenu();
                        break;
                    case "4":
                        ReportsMenu();
                        break;
                    case "5":
                        exit = true;
                        Console.WriteLine("\nThank you for using the Hotel Management System!");
                        break;
                    default:
                        Console.WriteLine("\nInvalid option. Please try again.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        static bool InitializeDatabase()
        {
            try
            {
                Console.WriteLine("Initializing database connection...");
                
                // Default connection settings - users can modify these
                string server = "localhost";
                string database = "hotel_management";
                string username = "root";
                string password = "";

                // Check if user wants to use custom settings
                Console.Write("Use default connection (localhost, hotel_management, root, no password)? (Y/n): ");
                string? useDefault = Console.ReadLine()?.Trim().ToLower();

                if (useDefault == "n" || useDefault == "no")
                {
                    Console.Write("Enter MySQL server (default: localhost): ");
                    string? inputServer = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(inputServer)) server = inputServer;

                    Console.Write("Enter database name (default: hotel_management): ");
                    string? inputDatabase = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(inputDatabase)) database = inputDatabase;

                    Console.Write("Enter username (default: root): ");
                    string? inputUsername = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(inputUsername)) username = inputUsername;

                    Console.Write("Enter password: ");
                    password = ReadPassword();
                }

                _dbConnection = new DatabaseConnection(server, database, username, password);

                Console.WriteLine("Testing database connection...");
                if (!_dbConnection.TestConnection())
                {
                    Console.WriteLine("Failed to connect to database. Please check your connection settings.");
                    return false;
                }

                Console.WriteLine("Connected successfully!");
                
                // Initialize database tables
                Console.WriteLine("Initializing database tables...");
                var initializer = new DatabaseInitializer(_dbConnection);
                initializer.InitializeDatabase();

                _hotelService = new HotelService(_dbConnection);
                Console.WriteLine("System initialized successfully!\n");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
                return false;
            }
        }

        static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }

        static void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine("\n╔════════════════════════════════════╗");
            Console.WriteLine("║   HOTEL MANAGEMENT SYSTEM (MySQL)  ║");
            Console.WriteLine("╠════════════════════════════════════╣");
            Console.WriteLine("║  1. Room Management                ║");
            Console.WriteLine("║  2. Guest Management               ║");
            Console.WriteLine("║  3. Reservation Management         ║");
            Console.WriteLine("║  4. Reports                        ║");
            Console.WriteLine("║  5. Exit                           ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.Write("\nSelect an option: ");
        }

        static void RoomManagementMenu()
        {
            Console.Clear();
            Console.WriteLine("\n===== Room Management =====");
            Console.WriteLine("1. View all rooms");
            Console.WriteLine("2. View available rooms");
            Console.WriteLine("3. Add new room");
            Console.WriteLine("4. Update room");
            Console.WriteLine("5. Delete room");
            Console.WriteLine("6. Back to main menu");
            Console.Write("\nSelect an option: ");
            
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    _hotelService?.DisplayRoomStatus();
                    break;
                case "2":
                    DisplayAvailableRooms();
                    break;
                case "3":
                    AddNewRoom();
                    break;
                case "4":
                    UpdateRoom();
                    break;
                case "5":
                    DeleteRoom();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("\nInvalid option.");
                    break;
            }
        }

        static void DisplayAvailableRooms()
        {
            var rooms = _hotelService?.GetAvailableRooms();
            Console.WriteLine("\n===== Available Rooms =====");
            Console.WriteLine($"{"Room#",-8} {"Type",-15} {"Floor",-7} {"Price/Night",-12} {"Capacity",-10}");
            Console.WriteLine(new string('-', 60));

            if (rooms != null)
            {
                foreach (var room in rooms)
                {
                    Console.WriteLine($"{room.RoomNumber,-8} {room.RoomType,-15} {room.Floor,-7} ${room.PricePerNight,-11:F2} {room.MaxOccupancy,-10}");
                }
            }
        }

        static void AddNewRoom()
        {
            Console.WriteLine("\n===== Add New Room =====");
            try
            {
                Console.Write("Room Number: ");
                string roomNumber = Console.ReadLine() ?? "";

                Console.Write("Room Type (Standard/Deluxe/Suite/Presidential): ");
                string roomType = Console.ReadLine() ?? "";

                Console.Write("Price per Night: ");
                decimal price = decimal.Parse(Console.ReadLine() ?? "0");

                Console.Write("Floor: ");
                int floor = int.Parse(Console.ReadLine() ?? "0");

                Console.Write("Max Occupancy: ");
                int maxOccupancy = int.Parse(Console.ReadLine() ?? "0");

                Console.Write("Description: ");
                string description = Console.ReadLine() ?? "";

                var room = new Room
                {
                    RoomNumber = roomNumber,
                    RoomType = roomType,
                    PricePerNight = price,
                    Floor = floor,
                    MaxOccupancy = maxOccupancy,
                    Description = description,
                    Status = "Available"
                };

                if (_hotelService?.AddRoom(room) == true)
                {
                    Console.WriteLine("\nRoom added successfully!");
                }
                else
                {
                    Console.WriteLine("\nFailed to add room.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        static void UpdateRoom()
        {
            Console.WriteLine("\n===== Update Room =====");
            Console.Write("Enter Room Number: ");
            string? roomNumber = Console.ReadLine();

            var room = _hotelService?.GetRoomByNumber(roomNumber ?? "");
            if (room == null)
            {
                Console.WriteLine("Room not found.");
                return;
            }

            Console.WriteLine($"\nCurrent Details:");
            Console.WriteLine($"Type: {room.RoomType}, Price: ${room.PricePerNight}, Status: {room.Status}");

            Console.Write("\nNew Price per Night (press Enter to keep current): ");
            string? priceInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(priceInput))
            {
                room.PricePerNight = decimal.Parse(priceInput);
            }

            Console.Write("New Status (Available/Occupied/Maintenance, press Enter to keep current): ");
            string? statusInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(statusInput))
            {
                room.Status = statusInput;
            }

            if (_hotelService?.UpdateRoom(room) == true)
            {
                Console.WriteLine("\nRoom updated successfully!");
            }
            else
            {
                Console.WriteLine("\nFailed to update room.");
            }
        }

        static void DeleteRoom()
        {
            Console.WriteLine("\n===== Delete Room =====");
            Console.Write("Enter Room Number: ");
            string? roomNumber = Console.ReadLine();

            var room = _hotelService?.GetRoomByNumber(roomNumber ?? "");
            if (room == null)
            {
                Console.WriteLine("Room not found.");
                return;
            }

            Console.Write($"Are you sure you want to delete room {roomNumber}? (yes/no): ");
            string? confirm = Console.ReadLine()?.ToLower();

            if (confirm == "yes")
            {
                if (_hotelService?.DeleteRoom(room.RoomId) == true)
                {
                    Console.WriteLine("\nRoom deleted successfully!");
                }
                else
                {
                    Console.WriteLine("\nFailed to delete room.");
                }
            }
        }

        static void GuestManagementMenu()
        {
            Console.Clear();
            Console.WriteLine("\n===== Guest Management =====");
            Console.WriteLine("1. View all guests");
            Console.WriteLine("2. Register new guest");
            Console.WriteLine("3. Update guest information");
            Console.WriteLine("4. Search guest");
            Console.WriteLine("5. Back to main menu");
            Console.Write("\nSelect an option: ");
            
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    _hotelService?.DisplayGuestList();
                    break;
                case "2":
                    RegisterNewGuest();
                    break;
                case "3":
                    UpdateGuest();
                    break;
                case "4":
                    SearchGuest();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("\nInvalid option.");
                    break;
            }
        }

        static void RegisterNewGuest()
        {
            Console.WriteLine("\n===== Register New Guest =====");
            try
            {
                Console.Write("First Name: ");
                string firstName = Console.ReadLine() ?? "";

                Console.Write("Last Name: ");
                string lastName = Console.ReadLine() ?? "";

                Console.Write("ID Number: ");
                string idNumber = Console.ReadLine() ?? "";

                Console.Write("Phone: ");
                string phone = Console.ReadLine() ?? "";

                Console.Write("Email: ");
                string email = Console.ReadLine() ?? "";

                Console.Write("Address: ");
                string address = Console.ReadLine() ?? "";

                var guest = new Guest
                {
                    FirstName = firstName,
                    LastName = lastName,
                    IdNumber = idNumber,
                    Phone = phone,
                    Email = email,
                    Address = address
                };

                int guestId = _hotelService?.RegisterGuest(guest) ?? 0;
                if (guestId > 0)
                {
                    Console.WriteLine($"\nGuest registered successfully! Guest ID: {guestId}");
                }
                else
                {
                    Console.WriteLine("\nFailed to register guest.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        static void UpdateGuest()
        {
            Console.WriteLine("\n===== Update Guest =====");
            Console.Write("Enter Guest ID: ");
            if (int.TryParse(Console.ReadLine(), out int guestId))
            {
                var guest = _hotelService?.GetGuestById(guestId);
                if (guest == null)
                {
                    Console.WriteLine("Guest not found.");
                    return;
                }

                Console.WriteLine($"\nCurrent: {guest.FirstName} {guest.LastName}, Phone: {guest.Phone}");

                Console.Write("New Phone (press Enter to keep current): ");
                string? phoneInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(phoneInput))
                {
                    guest.Phone = phoneInput;
                }

                Console.Write("New Email (press Enter to keep current): ");
                string? emailInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(emailInput))
                {
                    guest.Email = emailInput;
                }

                if (_hotelService?.UpdateGuest(guest) == true)
                {
                    Console.WriteLine("\nGuest information updated successfully!");
                }
                else
                {
                    Console.WriteLine("\nFailed to update guest information.");
                }
            }
        }

        static void SearchGuest()
        {
            Console.WriteLine("\n===== Search Guest =====");
            Console.Write("Enter ID Number: ");
            string? idNumber = Console.ReadLine();

            var guest = _hotelService?.GetGuestByIdNumber(idNumber ?? "");
            if (guest != null)
            {
                Console.WriteLine($"\nGuest Found:");
                Console.WriteLine($"ID: {guest.GuestId}");
                Console.WriteLine($"Name: {guest.FirstName} {guest.LastName}");
                Console.WriteLine($"ID Number: {guest.IdNumber}");
                Console.WriteLine($"Phone: {guest.Phone}");
                Console.WriteLine($"Email: {guest.Email}");
                Console.WriteLine($"Registration Date: {guest.RegistrationDate}");
            }
            else
            {
                Console.WriteLine("\nGuest not found.");
            }
        }

        static void ReservationManagementMenu()
        {
            Console.Clear();
            Console.WriteLine("\n===== Reservation Management =====");
            Console.WriteLine("1. View all reservations");
            Console.WriteLine("2. Make new reservation");
            Console.WriteLine("3. Check-in");
            Console.WriteLine("4. Check-out");
            Console.WriteLine("5. Cancel reservation");
            Console.WriteLine("6. Back to main menu");
            Console.Write("\nSelect an option: ");
            
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    _hotelService?.DisplayReservations();
                    break;
                case "2":
                    MakeReservation();
                    break;
                case "3":
                    CheckIn();
                    break;
                case "4":
                    CheckOut();
                    break;
                case "5":
                    CancelReservation();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("\nInvalid option.");
                    break;
            }
        }

        static void MakeReservation()
        {
            Console.WriteLine("\n===== Make New Reservation =====");
            try
            {
                Console.Write("Guest ID: ");
                int guestId = int.Parse(Console.ReadLine() ?? "0");

                // Check if guest exists
                var guest = _hotelService?.GetGuestById(guestId);
                if (guest == null)
                {
                    Console.WriteLine("Guest not found. Please register the guest first.");
                    return;
                }

                // Display available rooms
                DisplayAvailableRooms();

                Console.Write("\nRoom Number: ");
                string? roomNumber = Console.ReadLine();

                var room = _hotelService?.GetRoomByNumber(roomNumber ?? "");
                if (room == null)
                {
                    Console.WriteLine("Room not found.");
                    return;
                }

                Console.Write("Check-in Date (yyyy-mm-dd): ");
                string? checkInInput = Console.ReadLine();
                if (!DateTime.TryParse(checkInInput, out DateTime checkInDate))
                {
                    Console.WriteLine("Invalid date format. Please use yyyy-mm-dd format.");
                    return;
                }

                Console.Write("Check-out Date (yyyy-mm-dd): ");
                string? checkOutInput = Console.ReadLine();
                if (!DateTime.TryParse(checkOutInput, out DateTime checkOutDate))
                {
                    Console.WriteLine("Invalid date format. Please use yyyy-mm-dd format.");
                    return;
                }

                Console.Write("Special Requests (optional): ");
                string? specialRequests = Console.ReadLine();

                int reservationId = _hotelService?.MakeReservation(guestId, room.RoomId, checkInDate, checkOutDate, specialRequests) ?? 0;
                if (reservationId > 0)
                {
                    var reservation = _hotelService?.GetReservationById(reservationId);
                    Console.WriteLine($"\nReservation created successfully!");
                    Console.WriteLine($"Reservation ID: {reservationId}");
                    Console.WriteLine($"Total Amount: ${reservation?.TotalAmount:F2}");
                }
                else
                {
                    Console.WriteLine("\nFailed to create reservation.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        static void CheckIn()
        {
            Console.WriteLine("\n===== Check-In =====");
            Console.Write("Enter Reservation ID: ");
            if (int.TryParse(Console.ReadLine(), out int reservationId))
            {
                if (_hotelService?.CheckIn(reservationId) == true)
                {
                    Console.WriteLine("\nCheck-in successful!");
                }
                else
                {
                    Console.WriteLine("\nCheck-in failed. Please verify the reservation ID.");
                }
            }
        }

        static void CheckOut()
        {
            Console.WriteLine("\n===== Check-Out =====");
            Console.Write("Enter Reservation ID: ");
            if (int.TryParse(Console.ReadLine(), out int reservationId))
            {
                if (_hotelService?.CheckOut(reservationId) == true)
                {
                    Console.WriteLine("\nCheck-out successful!");
                }
                else
                {
                    Console.WriteLine("\nCheck-out failed. Please verify the reservation ID.");
                }
            }
        }

        static void CancelReservation()
        {
            Console.WriteLine("\n===== Cancel Reservation =====");
            Console.Write("Enter Reservation ID: ");
            if (int.TryParse(Console.ReadLine(), out int reservationId))
            {
                Console.Write("Are you sure you want to cancel this reservation? (yes/no): ");
                string? confirm = Console.ReadLine()?.ToLower();

                if (confirm == "yes")
                {
                    if (_hotelService?.CancelReservation(reservationId) == true)
                    {
                        Console.WriteLine("\nReservation cancelled successfully!");
                    }
                    else
                    {
                        Console.WriteLine("\nFailed to cancel reservation.");
                    }
                }
            }
        }

        static void ReportsMenu()
        {
            Console.Clear();
            Console.WriteLine("\n===== Reports =====");
            Console.WriteLine("1. Room Status Report");
            Console.WriteLine("2. Guest List");
            Console.WriteLine("3. Reservations Report");
            Console.WriteLine("4. Back to main menu");
            Console.Write("\nSelect an option: ");
            
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    _hotelService?.DisplayRoomStatus();
                    break;
                case "2":
                    _hotelService?.DisplayGuestList();
                    break;
                case "3":
                    _hotelService?.DisplayReservations();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("\nInvalid option.");
                    break;
            }
        }
    }
}
