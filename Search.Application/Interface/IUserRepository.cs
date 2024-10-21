using SearchBarWebAPI.Search.Application.Model;

namespace SearchBarWebAPI.Search.Application.Interface
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(string userName, string password);
    }
}
