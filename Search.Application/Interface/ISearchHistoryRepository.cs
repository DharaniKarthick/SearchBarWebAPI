using SearchBarWebAPI.Search.Application.Model;

namespace SearchBarWebAPI.Search.Application.Interface
{
    public interface ISearchHistoryRepository
    {
        Task<IEnumerable<SearchHistory>> GetSearchHistoryAsync(int userId);
    }
}
