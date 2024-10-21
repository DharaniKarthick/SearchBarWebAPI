using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SearchBarWebAPI.Search.API.Controllers;
using SearchBarWebAPI.Search.Application.Commands;
using SearchBarWebAPI.Search.Core.Model;

namespace SearchBarWebAPI.Tests.Controllers
{
    public class LoginControllerTest
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UserController _loginController;

        public LoginControllerTest()
        {
            _mediatorMock = new Mock<IMediator>();
            _loginController = new UserController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkResult()
        {
            // Arrange
            var loginCommand = new LoginCommand("dharani","123456");
            var loginResponse = new LoginResponse { Token = "some-jwt-token" };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<LoginCommand>(), default))
                .ReturnsAsync(loginResponse);

            // Act
            var result = await _loginController.Login(loginCommand);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            Assert.Equal(200,okResult.StatusCode);
            var response = okResult.Value.Should().BeOfType<LoginResponse>().Subject;
            Assert.Equal(loginResponse.Token, response.Token);

            // Verify that mediator send was called
            _mediatorMock.Verify(m => m.Send(It.IsAny<LoginCommand>(), default), Times.Once);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginCommand = new LoginCommand("invaliduser", "wrongpassword");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<LoginCommand>(), default))
                .ThrowsAsync(new UnauthorizedAccessException("Invalid credentials"));

            // Act
            Func<Task> action = async () => await _loginController.Login(loginCommand);

            // Assert
            await action.Should().ThrowAsync<UnauthorizedAccessException>()
                         .WithMessage("Invalid credentials");

            // Verify that mediator send was called
            _mediatorMock.Verify(m => m.Send(It.IsAny<LoginCommand>(), default), Times.Once);
        }
    }
}
