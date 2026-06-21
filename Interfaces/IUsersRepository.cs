using AppBackendCore2026.DTOs;

namespace AppBackendCore2026.Interfaces
{
    public interface IUsersRepository
    {
        Task<List<UserLightDto>> GetAll();
        Task<UserLightDto> AddUser(CreateUserDto user);
        Task<UserObj?> GetByEmail(string email);
        Task<UserObj?> GetById(int id);
        Task<UserLightDto?> UpdateUser(int id, UpdateUserDto user);
        Task<bool> DeleteUser(int userId);
    }
}