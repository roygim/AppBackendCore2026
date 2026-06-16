using Dapper;
using AppBackendCore2026.Data;

namespace AppBackendCore2026.Repositories
{
    public class UsersDataMysql: IUsersRepository
    {
        private readonly DapperContext _context;

        public UsersDataMysql(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<UserObj>> GetAll()
        {
            const string query = "SELECT id AS Id, firstname, lastname, email, password FROM users;";
            using var connection = _context.CreateConnection();
            var users = await connection.QueryAsync<UserObj>(query);
            return users.ToList();
        }
    }
}
