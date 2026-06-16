using AppBackendCore2026.Data;
using AppBackendCore2026.DTOs;
using Dapper;

namespace AppBackendCore2026.Repositories
{
    public class UsersDataMysql: IUsersRepository
    {
        private readonly UsersDbContext _context;

        public UsersDataMysql(UsersDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserLight>> GetAll()
        {
            const string query = "SELECT id, firstname, lastname, email FROM users;";
            using var connection = _context.CreateConnection();
            var users = await connection.QueryAsync<UserLight>(query);
            return users.ToList();
        }
    }
}
