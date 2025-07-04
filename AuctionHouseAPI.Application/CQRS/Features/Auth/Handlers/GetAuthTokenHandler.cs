﻿using AuctionHouseAPI.Application.CQRS.Features.Auth.Queries;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Shared.Exceptions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuctionHouseAPI.Application.CQRS.Features.Auth.Handlers
{
    public class GetAuthTokenHandler : IRequestHandler<GetAuthTokenQuery, string>
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<GetAuthTokenHandler> _logger;

        public GetAuthTokenHandler(IConfiguration configuration, IUserRepository userRepository, ILogger<GetAuthTokenHandler> logger)
        {
            _key = configuration["JwtSettings:Key"]!;
            _issuer = configuration["JwtSettings:Issuer"]!;
            _audience = configuration["JwtSettings:Audience"]!;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<string> Handle(GetAuthTokenQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.LoginDTO.Username) ?? throw new EntityDoesNotExistException($"User with given username ({request.LoginDTO.Username}) does not exist");
            if (!BCrypt.Net.BCrypt.Verify(request.LoginDTO.Password, user.Password))
            {
                throw new UnauthorizedAccessException();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
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
            _logger.LogInformation("User {UserId} has logged in", user.Id);
            return "Bearer " + new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
