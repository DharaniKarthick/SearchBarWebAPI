using SearchBarWebAPI.Search.Core.Model;

namespace SearchBarWebAPI.Search.Core.Interface
{
    public interface ISearchResultRepository
    {
        Task<IEnumerable<SearchResultResponse>> GetSearchResultAsync(int userId, int? searchHistoryId = null);
    }
}
