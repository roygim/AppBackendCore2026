using AppBackendCore2026.DTOs;

namespace AppBackendCore2026.Interfaces
{
    public interface IUsersService
    {
        Task<List<UserLight>> GetAll();
    }
}
