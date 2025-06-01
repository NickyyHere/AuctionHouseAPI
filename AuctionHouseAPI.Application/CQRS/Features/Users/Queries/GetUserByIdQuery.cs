using AuctionHouseAPI.Application.DTOs.Read;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Users.Queries
{
    public record GetUserByIdQuery(int id) : IRequest<UserDTO>;
}
