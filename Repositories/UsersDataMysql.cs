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

        public async Task<List<UserLightDto>> GetAll()
        {
            const string query = "SELECT id, firstname, lastname, email FROM users;";
            
            using var connection = _context.CreateConnection();
            var users = await connection.QueryAsync<UserLightDto>(query);
            
            return users.ToList();
        }

        public async Task<UserLightDto> AddUser(CreateUserDto user)
        {
            const string query = @"INSERT INTO users (firstname, lastname, email, password)
                                   VALUES (@firstname, @lastname, @email, @password);
                                   SELECT LAST_INSERT_ID();";

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.password, 12);

            using var connection = _context.CreateConnection();
            var newId = await connection.ExecuteScalarAsync<int>(query, new
            {
                user.firstname,
                user.lastname,
                user.email,
                password = hashedPassword
            });
            
            return new UserLightDto
            {
                id = newId,
                firstname = user.firstname,
                lastname = user.lastname,
                email = user.email
            };
        }

        public async Task<UserObj?> GetByEmail(string email)
        {
            const string query = "SELECT id, firstname, lastname, email, password AS Password FROM users WHERE email = @email;";

            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<UserObj>(query, new { email });
        }

        public async Task<UserObj?> GetById(int id)
        {
            const string query = "SELECT id, firstname, lastname, email, password FROM users WHERE id = @id;";

            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<UserObj>(query, new { id });
        }

        public async Task<UserLightDto?> GetUserLightById(int userId)
        {
            const string query = "SELECT id, firstname, lastname, email FROM users WHERE id = @userId;";

            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<UserLightDto>(query, new { userId });
        }

        public async Task<UserLightDto?> UpdateUser(int userId, UpdateUserDto user)
        {
            const string query = @"UPDATE users
                                   SET firstname = @firstname, lastname = @lastname
                                   WHERE id = @userId;";

            using var connection = _context.CreateConnection();

            var affected = await connection.ExecuteAsync(query, new
            {
                userId,
                user.firstname,
                user.lastname
            });

            if (affected == 0)
                return null;

            return await GetUserLightById(userId);
        }

        public async Task<bool> DeleteUser(int userId)
        {
            const string query = "DELETE FROM users WHERE id = @userId;";

            using var connection = _context.CreateConnection();
            var affected = await connection.ExecuteAsync(query, new { userId });

            return affected > 0;
        }
    }
}
