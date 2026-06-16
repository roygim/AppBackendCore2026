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

        public async Task<List<UserObj>> GetAll()
        {
            return await _usersRepository.GetAll();
        }
    }
}