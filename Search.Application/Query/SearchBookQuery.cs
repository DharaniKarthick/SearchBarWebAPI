using MediatR;
using SearchBarWebAPI.Search.Application.Model;

namespace SearchBarWebAPI.Search.Application.Query
{
    public class SearchBookQuery : IRequest<List<Book>>
    {
        public int UserId { get; }
        public string Query { get; }
        public string? Filter { get; } // Optional filter
        public string? Sort { get; }   // Optional sort term

        public SearchBookQuery(int userId, string query, string? filter, string? sort)
        {
            UserId = userId;
            Query = query;
            Filter = filter;
            Sort = sort;
        }
    }
}
