using MySqlConnector;

namespace AppBackendCore2026.Data
{
    public class UsersDbContext
    {
        private readonly string _connectionString;

        public UsersDbContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("csUsersMysql")
                ?? throw new InvalidOperationException("Connection string 'csUsersMysql' was not found.");
        }

        public MySqlConnection CreateConnection() => new MySqlConnection(_connectionString);
    }
}
