using AuctionHouseAPI.Application.CQRS.Features.Users.Commands;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Shared.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuctionHouseAPI.Application.CQRS.Features.Users.Handlers
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<DeleteUserHandler> _logger;
        public DeleteUserHandler(IUserService userService, IUserRepository userRepository, ILogger<DeleteUserHandler> logger)
        {
            _userService = userService;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.userId)
                ?? throw new EntityDoesNotExistException($"User with given id ({request.userId}) does not exist");
            await _userService.DeleteUserAsync(user);
            _logger.LogInformation("User {UserId} has been deleted", user.Id);
        }
    }
}
