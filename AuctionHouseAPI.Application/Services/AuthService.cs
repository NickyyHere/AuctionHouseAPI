using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuctionHouseAPI.Shared.Exceptions;
using AuctionHouseAPI.Domain.EFCore.Repositories.Interfaces;

namespace AuctionHouseAPI.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly IEFUserRepository _userRepository;
        public AuthService(IConfiguration configuration, IEFUserRepository userRepository)
        {
            _key = configuration["JwtSettings:Key"]!;
            _issuer = configuration["JwtSettings:Issuer"]!;
            _audience = configuration["JwtSettings:Audience"]!;
            _userRepository = userRepository;
        }

        public async Task<string> GetUserAuthenticationToken(LoginDTO loginDTO)
        {
            var user = await _userRepository.GetByUsernameAsync(loginDTO.Username) ?? throw new EntityDoesNotExistException($"User with given username ({loginDTO.Username}) does not exist");
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
