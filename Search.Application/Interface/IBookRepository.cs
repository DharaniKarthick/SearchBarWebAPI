using SearchBarWebAPI.Search.Application.Model;

namespace SearchBarWebAPI.Search.Application.Interface
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> SearchBooksAsync(int userId, string query, string? filter = null);
    }
}
