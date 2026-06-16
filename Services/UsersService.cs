using AppBackendCore2026.DTOs;
using System.Threading.Tasks;

namespace AppBackendCore2026.Services
{
    public class UsersService: IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<List<UserLight>> GetAll()
        {
            return await _usersRepository.GetAll();
        }
    }
}