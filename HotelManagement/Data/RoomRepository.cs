using HotelManagement.Models;
using MySql.Data.MySqlClient;

namespace HotelManagement.Data
{
    public class RoomRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public RoomRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public List<Room> GetAllRooms()
        {
            var rooms = new List<Room>();
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = "SELECT * FROM Rooms ORDER BY RoomNumber";
            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                rooms.Add(MapToRoom(reader));
            }

            return rooms;
        }

        public List<Room> GetAvailableRooms()
        {
            var rooms = new List<Room>();
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = "SELECT * FROM Rooms WHERE Status = 'Available' ORDER BY RoomNumber";
            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                rooms.Add(MapToRoom(reader));
            }

            return rooms;
        }

        public Room? GetRoomById(int roomId)
        {
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = "SELECT * FROM Rooms WHERE RoomId = @RoomId";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@RoomId", roomId);
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return MapToRoom(reader);
            }

            return null;
        }

        public Room? GetRoomByNumber(string roomNumber)
        {
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = "SELECT * FROM Rooms WHERE RoomNumber = @RoomNumber";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@RoomNumber", roomNumber);
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return MapToRoom(reader);
            }

            return null;
        }

        public bool AddRoom(Room room)
        {
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = @"INSERT INTO Rooms (RoomNumber, RoomType, PricePerNight, Status, Floor, MaxOccupancy, Description) 
                           VALUES (@RoomNumber, @RoomType, @PricePerNight, @Status, @Floor, @MaxOccupancy, @Description)";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@RoomNumber", room.RoomNumber);
            command.Parameters.AddWithValue("@RoomType", room.RoomType);
            command.Parameters.AddWithValue("@PricePerNight", room.PricePerNight);
            command.Parameters.AddWithValue("@Status", room.Status);
            command.Parameters.AddWithValue("@Floor", room.Floor);
            command.Parameters.AddWithValue("@MaxOccupancy", room.MaxOccupancy);
            command.Parameters.AddWithValue("@Description", room.Description);

            return command.ExecuteNonQuery() > 0;
        }

        public bool UpdateRoom(Room room)
        {
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = @"UPDATE Rooms SET RoomNumber = @RoomNumber, RoomType = @RoomType, 
                           PricePerNight = @PricePerNight, Status = @Status, Floor = @Floor, 
                           MaxOccupancy = @MaxOccupancy, Description = @Description 
                           WHERE RoomId = @RoomId";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@RoomId", room.RoomId);
            command.Parameters.AddWithValue("@RoomNumber", room.RoomNumber);
            command.Parameters.AddWithValue("@RoomType", room.RoomType);
            command.Parameters.AddWithValue("@PricePerNight", room.PricePerNight);
            command.Parameters.AddWithValue("@Status", room.Status);
            command.Parameters.AddWithValue("@Floor", room.Floor);
            command.Parameters.AddWithValue("@MaxOccupancy", room.MaxOccupancy);
            command.Parameters.AddWithValue("@Description", room.Description);

            return command.ExecuteNonQuery() > 0;
        }

        public bool UpdateRoomStatus(int roomId, string status)
        {
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = "UPDATE Rooms SET Status = @Status WHERE RoomId = @RoomId";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@RoomId", roomId);
            command.Parameters.AddWithValue("@Status", status);

            return command.ExecuteNonQuery() > 0;
        }

        public bool DeleteRoom(int roomId)
        {
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = "DELETE FROM Rooms WHERE RoomId = @RoomId";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@RoomId", roomId);

            return command.ExecuteNonQuery() > 0;
        }

        private Room MapToRoom(MySqlDataReader reader)
        {
            return new Room
            {
                RoomId = reader.GetInt32("RoomId"),
                RoomNumber = reader.GetString("RoomNumber"),
                RoomType = reader.GetString("RoomType"),
                PricePerNight = reader.GetDecimal("PricePerNight"),
                Status = reader.GetString("Status"),
                Floor = reader.GetInt32("Floor"),
                MaxOccupancy = reader.GetInt32("MaxOccupancy"),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? string.Empty : reader.GetString("Description")
            };
        }
    }
}
