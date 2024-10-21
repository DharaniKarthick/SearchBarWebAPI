using Microsoft.EntityFrameworkCore;
using SearchBarWebAPI.Search.Core.Interface;
using SearchBarWebAPI.Search.Core.Model;
using SearchBarWebAPI.Search.Infrastructure.Data;

namespace SearchBarWebAPI.Search.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserAsync(string userName, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == userName && u.Password == password);
        }
    }
}
