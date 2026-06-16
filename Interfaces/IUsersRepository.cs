using AppBackendCore2026.DTOs;

namespace AppBackendCore2026.Interfaces
{
    public interface IUsersRepository
    {
        Task<List<UserLight>> GetAll();
    }
}