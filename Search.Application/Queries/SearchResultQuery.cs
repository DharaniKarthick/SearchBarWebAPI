using MediatR;
using SearchBarWebAPI.Search.Core.Model;

namespace SearchBarWebAPI.Search.Application.Queries
{
    public class SearchResultQuery : IRequest<List<SearchResultResponse>>
    {
        public int UserId { get; set; }
        public int? SearchHistoryId { get; set; }
        public string? Sort { get; set; }
        public SearchResultQuery(int userId, int? searchHistoryId, string? sort)
        {
            UserId = userId;
            SearchHistoryId = searchHistoryId;
            Sort = sort;
        }

    }
}
