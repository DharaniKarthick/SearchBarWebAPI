using Microsoft.EntityFrameworkCore;
using SearchBarWebAPI.Search.Core.Model;

namespace SearchBarWebAPI.Search.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<SearchHistory> SearchHistory { get; set; }

        public DbSet<SearchResultResponse> SearchResultResponses { get; set; }
    }
}
