namespace AppBackendCore2026.Interfaces
{
    public interface IUsersService
    {
        Task<List<UserObj>> GetAll();
    }
}
