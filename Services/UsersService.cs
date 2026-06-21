using AppBackendCore2026.DTOs;
using System.Threading.Tasks;

namespace AppBackendCore2026.Services
{
    public class UsersService: IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ITokenService _tokenService;

        public UsersService(IUsersRepository usersRepository, ITokenService tokenService)
        {
            _usersRepository = usersRepository;
            _tokenService = tokenService;
        }

        public async Task<List<UserLightDto>> GetAll()
        {
            return await _usersRepository.GetAll();
        }

        public async Task<UserLightDto> AddUser(CreateUserDto user)
        {
            return await _usersRepository.AddUser(user);
        }

        public async Task<ResponseObj<UserLightDto>> GetUserById(int id)
        {
            var user = await _usersRepository.GetById(id);

            if (user == null)
                return new ResponseObj<UserLightDto> { success = false, error = ErrorType.UserNotFound };

            return new ResponseObj<UserLightDto> { success = true, data = new UserLightDto()
            {
                id = user.Id,
                email = user.email,
                firstname = user.firstname,
                lastname = user.lastname
            }};
        }

        public async Task<ResponseObj<LoginResultDto>> Login(string email, string password)
        {
            var user = await _usersRepository.GetByEmail(email);
            if (user == null)
                return new ResponseObj<LoginResultDto> { success = false, error = ErrorType.UserNotFound };

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                return new ResponseObj<LoginResultDto> { success = false, error = ErrorType.InvalidPassword };

            return new ResponseObj<LoginResultDto>
            {
                success = true,
                data = new LoginResultDto
                {
                    accessToken = _tokenService.CreateToken(user),
                    user = new UserLightDto
                    {
                        id = user.Id,
                        firstname = user.firstname,
                        lastname = user.lastname,
                        email = user.email
                    }
                }
            };
        }
    }
}