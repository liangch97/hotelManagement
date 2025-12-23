using HotelManagement.Models;
using MySql.Data.MySqlClient;

namespace HotelManagement.Data
{
    public class ReservationRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public ReservationRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public List<Reservation> GetAllReservations()
        {
            var reservations = new List<Reservation>();
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = "SELECT * FROM Reservations ORDER BY ReservationDate DESC";
            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                reservations.Add(MapToReservation(reader));
            }

            return reservations;
        }

        public Reservation? GetReservationById(int reservationId)
        {
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = "SELECT * FROM Reservations WHERE ReservationId = @ReservationId";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ReservationId", reservationId);
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return MapToReservation(reader);
            }

            return null;
        }

        public List<Reservation> GetReservationsByGuest(int guestId)
        {
            var reservations = new List<Reservation>();
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = "SELECT * FROM Reservations WHERE GuestId = @GuestId ORDER BY ReservationDate DESC";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@GuestId", guestId);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                reservations.Add(MapToReservation(reader));
            }

            return reservations;
        }

        public List<Reservation> GetReservationsByRoom(int roomId)
        {
            var reservations = new List<Reservation>();
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = "SELECT * FROM Reservations WHERE RoomId = @RoomId ORDER BY CheckInDate";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@RoomId", roomId);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                reservations.Add(MapToReservation(reader));
            }

            return reservations;
        }

        public int AddReservation(Reservation reservation)
        {
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = @"INSERT INTO Reservations (GuestId, RoomId, CheckInDate, CheckOutDate, Status, TotalAmount, ReservationDate, SpecialRequests) 
                           VALUES (@GuestId, @RoomId, @CheckInDate, @CheckOutDate, @Status, @TotalAmount, @ReservationDate, @SpecialRequests);
                           SELECT LAST_INSERT_ID();";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@GuestId", reservation.GuestId);
            command.Parameters.AddWithValue("@RoomId", reservation.RoomId);
            command.Parameters.AddWithValue("@CheckInDate", reservation.CheckInDate);
            command.Parameters.AddWithValue("@CheckOutDate", reservation.CheckOutDate);
            command.Parameters.AddWithValue("@Status", reservation.Status);
            command.Parameters.AddWithValue("@TotalAmount", reservation.TotalAmount);
            command.Parameters.AddWithValue("@ReservationDate", reservation.ReservationDate);
            command.Parameters.AddWithValue("@SpecialRequests", (object?)reservation.SpecialRequests ?? DBNull.Value);

            return Convert.ToInt32(command.ExecuteScalar());
        }

        public bool UpdateReservation(Reservation reservation)
        {
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = @"UPDATE Reservations SET GuestId = @GuestId, RoomId = @RoomId, 
                           CheckInDate = @CheckInDate, CheckOutDate = @CheckOutDate, Status = @Status, 
                           TotalAmount = @TotalAmount, SpecialRequests = @SpecialRequests 
                           WHERE ReservationId = @ReservationId";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ReservationId", reservation.ReservationId);
            command.Parameters.AddWithValue("@GuestId", reservation.GuestId);
            command.Parameters.AddWithValue("@RoomId", reservation.RoomId);
            command.Parameters.AddWithValue("@CheckInDate", reservation.CheckInDate);
            command.Parameters.AddWithValue("@CheckOutDate", reservation.CheckOutDate);
            command.Parameters.AddWithValue("@Status", reservation.Status);
            command.Parameters.AddWithValue("@TotalAmount", reservation.TotalAmount);
            command.Parameters.AddWithValue("@SpecialRequests", (object?)reservation.SpecialRequests ?? DBNull.Value);

            return command.ExecuteNonQuery() > 0;
        }

        public bool UpdateReservationStatus(int reservationId, string status)
        {
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = "UPDATE Reservations SET Status = @Status WHERE ReservationId = @ReservationId";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ReservationId", reservationId);
            command.Parameters.AddWithValue("@Status", status);

            return command.ExecuteNonQuery() > 0;
        }

        public bool DeleteReservation(int reservationId)
        {
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = "DELETE FROM Reservations WHERE ReservationId = @ReservationId";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ReservationId", reservationId);

            return command.ExecuteNonQuery() > 0;
        }

        private Reservation MapToReservation(MySqlDataReader reader)
        {
            return new Reservation
            {
                ReservationId = reader.GetInt32("ReservationId"),
                GuestId = reader.GetInt32("GuestId"),
                RoomId = reader.GetInt32("RoomId"),
                CheckInDate = reader.GetDateTime("CheckInDate"),
                CheckOutDate = reader.GetDateTime("CheckOutDate"),
                Status = reader.GetString("Status"),
                TotalAmount = reader.GetDecimal("TotalAmount"),
                ReservationDate = reader.GetDateTime("ReservationDate"),
                SpecialRequests = reader.IsDBNull(reader.GetOrdinal("SpecialRequests")) ? null : reader.GetString("SpecialRequests")
            };
        }
    }
}
