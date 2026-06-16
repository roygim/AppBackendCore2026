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
    }
}
