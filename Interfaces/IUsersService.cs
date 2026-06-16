using AppBackendCore2026.DTOs;

namespace AppBackendCore2026.Interfaces
{
    public interface IUsersService
    {
        Task<List<UserLightDto>> GetAll();
        Task<UserLightDto> AddUser(CreateUserDto user);
    }
}
