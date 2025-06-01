using AuctionHouseAPI.Application.DTOs.Update;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Users.Commands
{
    public record UpdateUserCommand(UpdateUserDTO updateUserDTO, int userId) : IRequest;
}
