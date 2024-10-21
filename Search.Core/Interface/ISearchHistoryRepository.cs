using SearchBarWebAPI.Search.Core.Model;

namespace SearchBarWebAPI.Search.Core.Interface
{
    public interface ISearchHistoryRepository
    {
        Task<IEnumerable<SearchHistory>> GetSearchHistoryAsync(int userId);
    }
}
