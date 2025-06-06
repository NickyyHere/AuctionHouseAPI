using AuctionHouseAPI.Application.CQRS.Features.Users.Commands;
using AuctionHouseAPI.Application.DTOs.Update;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AutoMapper;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Users.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public CreateUserHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request.CreateUserDTO);
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            return await _userService.CreateUserAsync(user);
        }
    }
}
