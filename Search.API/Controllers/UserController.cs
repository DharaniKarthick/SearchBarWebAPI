using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SearchBarWebAPI.Search.Application.Commands;
using Serilog;

namespace SearchBarWebAPI.Search.API.Controllers
{
    /// <summary>
    /// Controller for managing user-related operations, such as login.
    /// </summary>
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator instance used for sending commands.</param>
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Logs in a user with the provided login credentials.
        /// </summary>
        /// <param name="command">The command containing the user's login information.</param>
        /// <returns>An IActionResult containing the login response.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            if (string.IsNullOrEmpty(command?.Username) || string.IsNullOrEmpty(command.Password))
            {
                return BadRequest("Username and password must be provided.");
            }
            Log.Information("Login request started for user with Username: {username}", command.Username);
            var response = await _mediator.Send(command);
            Log.Information("Login request completed for user with Username: {username}", command.Username);
            return Ok(response);
        }

    }
}
