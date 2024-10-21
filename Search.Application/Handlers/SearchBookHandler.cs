using MediatR;
using SearchBarWebAPI.Search.Application.Interface;
using SearchBarWebAPI.Search.Application.Model;
using SearchBarWebAPI.Search.Application.Query;

namespace SearchBarWebAPI.Search.Application.Handlers
{
    public class SearchBookHandler : IRequestHandler<SearchBookQuery, List<Book>>
    {
        private readonly IBookRepository _bookRepository;

        public SearchBookHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<Book>> Handle(SearchBookQuery request, CancellationToken cancellationToken)
        {
            var books = await _bookRepository.SearchBooksAsync(request.UserId, request.Query, request.Filter);
            if (!string.IsNullOrEmpty(request.Sort))
            {
                books = SortBooks(books, request.Sort);
            }

            // Map to response
            return books.Select(b => new Book
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Summary = b.Summary,
                Tags = b.Tags,
                Genre = b.Genre,
                PublishDate = b.PublishDate,
                Ratings = b.Ratings,
                Price = b.Price
            }).ToList();
        }

        private IEnumerable<Book> SortBooks(IEnumerable<Book> books, string sortTerm)
        {
            return sortTerm.ToLower() switch
            {
                "title" => books.OrderBy(b => b.Title),
                "author" => books.OrderBy(b => b.Author),
                "summary" => books.OrderBy(b => b.Summary),
                "tags" => books.OrderBy(b => b.Tags),
                _ => books
            };
        }
    }
}
