using AuctionHouseAPI.Application.CQRS.Features.Users.Commands;
using AuctionHouseAPI.Application.DTOs.Update;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuctionHouseAPI.Application.CQRS.Features.Users.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateUserHandler> _logger;
        public CreateUserHandler(IUserService userService, IMapper mapper, ILogger<CreateUserHandler> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request.CreateUserDTO);
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var userId = await _userService.CreateUserAsync(user);
            _logger.LogInformation("User {UserId} has been created", userId);
            return userId;
        }
    }
}
