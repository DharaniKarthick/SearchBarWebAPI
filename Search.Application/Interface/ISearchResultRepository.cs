using SearchBarWebAPI.Search.Application.Model;

namespace SearchBarWebAPI.Search.Application.Interface
{
    public interface ISearchResultRepository
    {
        Task<IEnumerable<SearchResultResponse>> GetSearchResultAsync(int userId, int? searchHistoryId = null);
    }
}
