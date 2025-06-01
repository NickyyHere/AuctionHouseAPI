using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Users.Commands
{
    public record DeleteUserCommand(int userId) : IRequest;
}
