using SearchBarWebAPI.Search.Core.Model;

namespace SearchBarWebAPI.Search.Core.Interface
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> SearchBooksAsync(int userId, string query, string? filter = null);
    }
}
