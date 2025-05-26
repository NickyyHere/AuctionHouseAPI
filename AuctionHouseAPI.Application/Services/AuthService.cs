using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Repositories.interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuctionHouseAPI.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly IUserRepository _userRepository;
        public AuthService(IConfiguration configuration, IUserRepository userRepository)
        {
            _key = configuration["JwtSettings:Key"]!;
            _issuer = configuration["JwtSettings:Issuer"]!;
            _audience = configuration["JwtSettings:Audience"]!;
            _userRepository = userRepository;
        }

        public async Task<string> GetUserAuthenticationToken(LoginDTO loginDTO)
        {
            var user = await _userRepository.GetUserByUsername(loginDTO.Username);
            if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password))
                throw new UnauthorizedAccessException();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
                );
            return "Bearer " + new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
