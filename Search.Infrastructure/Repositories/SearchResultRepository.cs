using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SearchBarWebAPI.Search.Application.Interface;
using SearchBarWebAPI.Search.Application.Model;
using SearchBarWebAPI.Search.Infrastructure.Data;

namespace SearchBarWebAPI.Search.Infrastructure.Repositories
{
    public class SearchResultRepository : ISearchResultRepository
    {
        private readonly AppDbContext _context;

        public SearchResultRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SearchResultResponse>> GetSearchResultAsync(int userId, int? searchHistoryId = null)
        {
            var userIdParam = new SqlParameter("@UserId", userId);
            var searchHistoryIdParam = new SqlParameter("@SearchHistoryId", (object)searchHistoryId ?? DBNull.Value); // Handle nullable parameter

            var result = await _context.Set<SearchResultResponse>()
                .FromSqlRaw("EXEC GetSearchResultsDetails @UserId, @SearchHistoryId", userIdParam, searchHistoryIdParam)
                .ToListAsync();

            return result;
        }
    }
}
