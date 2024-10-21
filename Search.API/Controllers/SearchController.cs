using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SearchBarWebAPI.Search.Application.Query;
using SearchBarWebAPI.Search.Application.Utility;
using Serilog;
using System.IdentityModel.Tokens.Jwt;

namespace SearchBarWebAPI.Search.API.Controllers
{
    /// <summary>
    /// Controller for handling search-related operations, including searching for books and retrieving search history.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator instance used for sending requests.</param>
        public SearchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Searches for books based on the specified query, filter, and sort options.
        /// </summary>
        /// <param name="query">The search query entered by the user.</param>
        /// <param name="filter">Optional filter criteria for the search.</param>
        /// <param name="sort">Optional sorting criteria for the search results.</param>
        /// <returns>An IActionResult containing the search results.</returns>
        [HttpGet("books")]
        public async Task<IActionResult> SearchBooks([FromQuery] string query, [FromQuery] string? filter = null, [FromQuery] string? sort = null)
        {
            Log.Information("Search request started with query:{query},filter:{filter},sort:{sort}", query, filter, sort);
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Get UserId from the token
            int userId = GetUserId(token);

            if (!IsValidEnumValue<BookFilterEnum>(filter))
            {
                throw new BadRequestException("Invalid filter input. Allowed values are: title, author, summary, tags.");
            }

            if (!IsValidEnumValue<BookSortEnum>(sort))
            {
                throw new BadRequestException("Invalid sort input. Allowed values are: title, author, summary, tags.");
            }

            var result = await _mediator.Send(new SearchBookQuery(userId, query, filter, sort));
            if (result.Count() <= 0)
            {
                throw new NotFoundException("Query cannot be found. Please try again with different query");
            }
            Log.Information("Search request completed with query:{query},filter:{filter},sort:{sort},totalCount:{totalCount}", query, filter, sort, result.Count);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves the search history for the authenticated user.
        /// </summary>
        /// <returns>An IActionResult containing the search history.</returns>
        [HttpGet("history")] 
        public async Task<IActionResult> GetSearchHistory()
        {
            Log.Information("Search History request started");
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Get UserId from the token
            int userId = GetUserId(token);
            var history = await _mediator.Send(new SearchHistoryQuery(userId));
            if (history.Count() <= 0)
            {
                throw new NotFoundException("There is no history for the user.");
            }
            Log.Information("Search History request completed with total result of: {count}", history.Count);
            return Ok(history);
        }

        /// <summary>
        /// Retrieves the search results based on the provided search history ID and optional sort parameter.
        /// </summary>
        /// <param name="searchHistoryId">The ID of the search history record.</param>
        /// <param name="sort">Optional sorting criteria for the search results.</param>
        /// <returns>An IActionResult containing the search results.</returns>
        [HttpGet("searchresult")]
        public async Task<IActionResult> GetSearchResult([FromQuery] int? searchHistoryId = null, [FromQuery] string? sort = null)
        {
            Log.Information("Search result request started with searchHistory of :{searchHistory},sort:{sort}", searchHistoryId, sort);
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Get UserId from the token
            int userId = GetUserId(token);

            if (!IsValidEnumValue<SearchResultSortEnum>(sort))
            {
                throw new BadRequestException("Invalid sort input. Allowed values are: title, author, ratings, relevancescore.");
            }
            var result = await _mediator.Send(new SearchResultQuery(userId, searchHistoryId, sort));
            if (result.Count() <= 0)
            {
                throw new NotFoundException("No searchresults found");
            }
            Log.Information("Search result request completed with searchHistory of :{searchHistory},sort:{sort},totalCount:{totalCount}", searchHistoryId, sort, result.Count);
            return Ok(result);
        }

        /// <summary>
        /// Extracts the UserId from the provided JWT token.
        /// </summary>
        /// <param name="token">The JWT token from which to extract the UserId.</param>
        /// <returns>The extracted UserId.</returns>        [NonAction]
        private int GetUserId(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Get the user id (or any other claim)
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (!int.TryParse(userId, out var parsedUserId) || parsedUserId == 0)
            {
                throw new UnauthorizedAccessException("Invalid or missing UserId in the token.");
            }

            return parsedUserId;
        }

        /// <summary>
        /// Checks if the provided value is a valid enum value.
        /// </summary>
        /// <typeparam name="T">The enum type to validate against.</typeparam>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if the value is valid; otherwise, false.</returns>
        private bool IsValidEnumValue<T>(string? value) where T : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value)) return true; // Allow null or empty values
            return Enum.TryParse(typeof(T), value, true, out _); // Case-insensitive check
        }
    }
}
