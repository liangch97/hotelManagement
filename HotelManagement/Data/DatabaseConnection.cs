using MySql.Data.MySqlClient;

namespace HotelManagement.Data
{
    public class DatabaseConnection
    {
        private readonly string _connectionString;

        public DatabaseConnection(string server, string database, string username, string password)
        {
            _connectionString = $"Server={server};Database={database};Uid={username};Pwd={password};";
        }

        public DatabaseConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public bool TestConnection()
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
