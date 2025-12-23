using HotelManagement.Models;
using MySql.Data.MySqlClient;

namespace HotelManagement.Data
{
    public class GuestRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public GuestRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public List<Guest> GetAllGuests()
        {
            var guests = new List<Guest>();
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = "SELECT * FROM Guests ORDER BY LastName, FirstName";
            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                guests.Add(MapToGuest(reader));
            }

            return guests;
        }

        public Guest? GetGuestById(int guestId)
        {
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = "SELECT * FROM Guests WHERE GuestId = @GuestId";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@GuestId", guestId);
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return MapToGuest(reader);
            }

            return null;
        }

        public Guest? GetGuestByIdNumber(string idNumber)
        {
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = "SELECT * FROM Guests WHERE IdNumber = @IdNumber";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@IdNumber", idNumber);
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return MapToGuest(reader);
            }

            return null;
        }

        public int AddGuest(Guest guest)
        {
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = @"INSERT INTO Guests (FirstName, LastName, IdNumber, Phone, Email, Address, RegistrationDate) 
                           VALUES (@FirstName, @LastName, @IdNumber, @Phone, @Email, @Address, @RegistrationDate);
                           SELECT LAST_INSERT_ID();";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@FirstName", guest.FirstName);
            command.Parameters.AddWithValue("@LastName", guest.LastName);
            command.Parameters.AddWithValue("@IdNumber", guest.IdNumber);
            command.Parameters.AddWithValue("@Phone", guest.Phone);
            command.Parameters.AddWithValue("@Email", guest.Email);
            command.Parameters.AddWithValue("@Address", guest.Address);
            command.Parameters.AddWithValue("@RegistrationDate", guest.RegistrationDate);

            return Convert.ToInt32(command.ExecuteScalar());
        }

        public bool UpdateGuest(Guest guest)
        {
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = @"UPDATE Guests SET FirstName = @FirstName, LastName = @LastName, 
                           IdNumber = @IdNumber, Phone = @Phone, Email = @Email, Address = @Address 
                           WHERE GuestId = @GuestId";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@GuestId", guest.GuestId);
            command.Parameters.AddWithValue("@FirstName", guest.FirstName);
            command.Parameters.AddWithValue("@LastName", guest.LastName);
            command.Parameters.AddWithValue("@IdNumber", guest.IdNumber);
            command.Parameters.AddWithValue("@Phone", guest.Phone);
            command.Parameters.AddWithValue("@Email", guest.Email);
            command.Parameters.AddWithValue("@Address", guest.Address);

            return command.ExecuteNonQuery() > 0;
        }

        public bool DeleteGuest(int guestId)
        {
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = "DELETE FROM Guests WHERE GuestId = @GuestId";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@GuestId", guestId);

            return command.ExecuteNonQuery() > 0;
        }

        private Guest MapToGuest(MySqlDataReader reader)
        {
            return new Guest
            {
                GuestId = reader.GetInt32("GuestId"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                IdNumber = reader.GetString("IdNumber"),
                Phone = reader.GetString("Phone"),
                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? string.Empty : reader.GetString("Email"),
                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? string.Empty : reader.GetString("Address"),
                RegistrationDate = reader.GetDateTime("RegistrationDate")
            };
        }
    }
}
