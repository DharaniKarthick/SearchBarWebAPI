using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moq;
using SearchBarWebAPI.Search.API.Controllers;
using SearchBarWebAPI.Search.Application.Queries;
using SearchBarWebAPI.Search.Core.Model;
using SearchBarWebAPI.Search.Core.Utility;
using System.IdentityModel.Tokens.Jwt;

namespace Search.Test
{
    public class SearchControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly SearchController _controller;

        public SearchControllerTests()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new SearchController(_mockMediator.Object);
        }

        [Fact]
        public async Task SearchBooks_ReturnsOkResult_WhenBooksFound()
        {
            // Arrange
            string query = "test";
            int userId = 1; // Example UserId
            string token = GenerateJwtToken(userId); // Method to generate a JWT token with UserId claim
            var expectedBooks = new List<Book>
            {
                new Book { Id = 1, Title = "Test Book", Author = "Author Name" }
            };

            // Mocking Mediator
            _mockMediator
                .Setup(m => m.Send(It.IsAny<SearchBookQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedBooks);

            _controller.ControllerContext.HttpContext = new DefaultHttpContext(); // Create HttpContext
            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Act
            var result = await _controller.SearchBooks(query, null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBooks = Assert.IsType<List<Book>>(okResult.Value);
            Assert.Equal(expectedBooks.Count, returnedBooks.Count);
        }

        [Fact]
        public async Task SearchBooks_ReturnsBadRequest_WhenInvalidFilter()
        {
            // Arrange
            string query = "test";
            string invalidFilter = "invalidFilter";
            string token = GenerateJwtToken(1);

            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _controller.SearchBooks(query, invalidFilter, null));
            Assert.Equal("Invalid filter input. Allowed values are: title, author, summary, tags.", exception.Message);
        }

        [Fact]
        public async Task SearchBooks_ReturnsNotFound_WhenNoBooksFound()
        {
            // Arrange
            string query = "nonexistent";
            string token = GenerateJwtToken(1);

            _mockMediator
                .Setup(m => m.Send(It.IsAny<SearchBookQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Book>()); // No books found

            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _controller.SearchBooks(query, null, null));
            Assert.Equal("Query cannot be found. Please try again with different query", exception.Message);
        }

        [Fact]
        public async Task GetSearchHistory_ReturnsOkResult_WhenHistoryExists()
        {
            // Arrange
            int userId = 1;
            string token = GenerateJwtToken(userId);
            var expectedHistory = new List<SearchHistory> { new SearchHistory { /* properties */ } };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<SearchHistoryQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedHistory);

            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Act
            var result = await _controller.GetSearchHistory();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedHistory = Assert.IsType<List<SearchHistory>>(okResult.Value);
            Assert.Equal(expectedHistory.Count, returnedHistory.Count);
        }

        [Fact]
        public async Task GetSearchHistory_ReturnsNotFound_WhenNoHistoryFound()
        {
            // Arrange
            int userId = 1;
            string token = GenerateJwtToken(userId);

            _mockMediator
                .Setup(m => m.Send(It.IsAny<SearchHistoryQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SearchHistory>()); // No history

            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _controller.GetSearchHistory());
            Assert.Equal("There is no history for the user.", exception.Message);
        }

        private string GenerateJwtToken(int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Configure your token here
                // Include claims such as UserId
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
