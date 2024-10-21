using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SearchBarWebAPI.Search.Application.Interface;
using SearchBarWebAPI.Search.Application.Model;
using SearchBarWebAPI.Search.Infrastructure.Data;

namespace SearchBarWebAPI.Search.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(int userId, string query, string? filter = null)
        {
            var searchDataParam = new SqlParameter("@SearchData", query);
            var filterParam = new SqlParameter("@Filter", (object)filter ?? DBNull.Value);
            var userIdParam = new SqlParameter("@UserId", userId);
            var result = await _context.Books.FromSqlRaw("EXEC GetSearchResults @UserId,@SearchData,@Filter", userIdParam, searchDataParam, filterParam).ToListAsync();
            return result;
        }
    }
}
