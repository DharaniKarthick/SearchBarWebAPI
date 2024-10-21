using MediatR;
using SearchBarWebAPI.Search.Application.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SearchBarWebAPI.Search.Application.Query
{
    public class SearchHistoryQuery : IRequest<List<SearchHistory>>
    {
        public int UserId { get; set; }
        public SearchHistoryQuery(int userId)
        {
            UserId = userId;
        }
    }
}
