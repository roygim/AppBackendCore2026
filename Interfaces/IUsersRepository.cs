namespace AppBackendCore2026.Interfaces
{
    public interface IUsersRepository
    {
        Task<List<UserObj>> GetAll();
    }
}