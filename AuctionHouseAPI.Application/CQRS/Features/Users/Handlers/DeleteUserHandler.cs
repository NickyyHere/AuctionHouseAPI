using AuctionHouseAPI.Application.CQRS.Features.Users.Commands;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Shared.Exceptions;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Users.Handlers
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        public DeleteUserHandler(IUserService userService, IUserRepository userRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.userId)
                ?? throw new EntityDoesNotExistException($"User with given id ({request.userId}) does not exist");
            await _userService.DeleteUserAsync(user);
        }
    }
}
