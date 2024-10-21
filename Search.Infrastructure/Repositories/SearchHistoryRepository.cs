using Microsoft.EntityFrameworkCore;
using SearchBarWebAPI.Search.Core.Interface;
using SearchBarWebAPI.Search.Core.Model;
using SearchBarWebAPI.Search.Infrastructure.Data;

namespace SearchBarWebAPI.Search.Infrastructure.Repositories
{
    public class SearchHistoryRepository : ISearchHistoryRepository
    {
        private readonly AppDbContext _context;

        public SearchHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SearchHistory>> GetSearchHistoryAsync(int userId)
        {
            var result = await _context.SearchHistory
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.SearchDate)
                .ToListAsync();
            return result;
        }
    }
}
