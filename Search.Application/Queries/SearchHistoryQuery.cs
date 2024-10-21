using MediatR;
using SearchBarWebAPI.Search.Core.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SearchBarWebAPI.Search.Application.Queries
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
