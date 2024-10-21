using MediatR;
using SearchBarWebAPI.Search.Application.Queries;
using SearchBarWebAPI.Search.Core.Interface;
using SearchBarWebAPI.Search.Core.Model;

namespace SearchBarWebAPI.Search.Application.Handlers
{
    public class SearchResultHandler : IRequestHandler<SearchResultQuery, List<SearchResultResponse>>
    {
        private readonly ISearchResultRepository _searchResultRepository;

        public SearchResultHandler(ISearchResultRepository searchResultRepository)
        {
            _searchResultRepository = searchResultRepository;
        }

        public async Task<List<SearchResultResponse>> Handle(SearchResultQuery request, CancellationToken cancellationToken)
        {
            var result = await _searchResultRepository.GetSearchResultAsync(request.UserId, request.SearchHistoryId);
            if (!string.IsNullOrEmpty(request.Sort))
            {
                result = SortResults(result, request.Sort);
            }

            // Map to response
            return result.Select(b => new SearchResultResponse
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Summary = b.Summary,
                Tags = b.Tags,
                Genre = b.Genre,
                PublishDate = b.PublishDate,
                Ratings = b.Ratings,
                Price = b.Price,
                SearchHistoryId = b.SearchHistoryId,
                BookId = b.BookId,
                RelevanceScore = b.RelevanceScore,
                SearchDate = b.SearchDate,
            }).ToList();
        }

        private IEnumerable<SearchResultResponse> SortResults(IEnumerable<SearchResultResponse> result, string sortTerm)
        {
            return sortTerm.ToLower() switch
            {
                "title" => result.OrderBy(b => b.Title),
                "author" => result.OrderBy(b => b.Author),
                "ratings" => result.OrderByDescending(b => b.Ratings),
                "RelevanceScore" => result.OrderByDescending(b => b.RelevanceScore),
                _ => result // Default case, no sorting
            };
        }
    }
}
