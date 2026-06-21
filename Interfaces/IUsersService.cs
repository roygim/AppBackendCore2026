using AppBackendCore2026.DTOs;

namespace AppBackendCore2026.Interfaces
{
    public interface IUsersService
    {
        Task<ResponseObj<List<UserLightDto>>> GetAll();
        Task<ResponseObj<UserLightDto>> AddUser(CreateUserDto user);
        Task<ResponseObj<LoginResultDto>> Login(string email, string password);
        Task<ResponseObj<UserLightDto>> GetUserById(int id);
        Task<ResponseObj<UserLightDto>> UpdateUser(int id, UpdateUserDto user);
        Task<ResponseObj<object>> DeleteUser(int userId);
    }
}
