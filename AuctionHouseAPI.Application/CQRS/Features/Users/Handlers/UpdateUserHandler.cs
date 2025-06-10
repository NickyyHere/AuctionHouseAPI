using AuctionHouseAPI.Application.CQRS.Features.Users.Commands;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Shared.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuctionHouseAPI.Application.CQRS.Features.Users.Handlers
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly ILogger<UpdateUserHandler> _logger;
        public UpdateUserHandler(IUserRepository userRepository, IUserService userService, ILogger<UpdateUserHandler> logger)
        {
            _userRepository = userRepository;
            _userService = userService;
            _logger = logger;
        }

        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.userId)
                ?? throw new EntityDoesNotExistException($"User with given id ({request.userId}) does not exist");
            await _userService.UpdateUserAsync(user, request.updateUserDTO);
            _logger.LogInformation("User {UserId} has been updated", user.Id);
        }
    }
}
