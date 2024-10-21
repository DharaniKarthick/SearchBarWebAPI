using SearchBarWebAPI.Search.Core.Model;

namespace SearchBarWebAPI.Search.Core.Interface
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(string userName, string password);
    }
}
