using MediatR;
using Microsoft.IdentityModel.Tokens;
using SearchBarWebAPI.Search.Application.Commands;
using SearchBarWebAPI.Search.Core.Interface;
using SearchBarWebAPI.Search.Core.Model;
using SearchBarWebAPI.Search.Core.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SearchBarWebAPI.Search.Application.Handlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public LoginHandler(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserAsync(request.Username, request.Password);
            LoginResponse response = new LoginResponse();
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }
            else
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, request.Username),
                    new Claim("UserId", user.Id.ToString())
                };
                response.Token = GenerateAccessToken(claims);
                response.UserId = user.Id;
                return response;
            }
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection(AuthConstants.appJWTSection).GetSection(AuthConstants.appIssuerSigningKey).Value));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var expirytime = DateTime.UtcNow.AddMinutes(Convert.ToInt16(_configuration.GetSection(AuthConstants.appJWTSection).GetSection(AuthConstants.appTokenExpiryTimeInMins).Value));
            var tokeOptions = new JwtSecurityToken(
                issuer: _configuration.GetSection(AuthConstants.appJWTSection).GetSection(AuthConstants.appIssuer).Value,
                audience: _configuration.GetSection(AuthConstants.appJWTSection).GetSection(AuthConstants.appAudience).Value,
                claims: claims,
                expires: expirytime,
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }
    }
}
