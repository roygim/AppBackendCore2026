using AppBackendCore2026.DTOs;

namespace AppBackendCore2026.Interfaces
{
    public interface IUsersRepository
    {
        Task<List<UserLightDto>> GetAll();
        Task<UserLightDto> AddUser(CreateUserDto user);
    }
}