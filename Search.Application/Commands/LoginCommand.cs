using MediatR;
using SearchBarWebAPI.Search.Application.Model;

namespace SearchBarWebAPI.Search.Application.Commands
{
    public class LoginCommand : IRequest<LoginResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginCommand(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
