using AuctionHouseAPI.Application.DTOs.Create;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Users.Commands
{
    public record CreateUserCommand(CreateUserDTO CreateUserDTO) : IRequest<int>;
}
