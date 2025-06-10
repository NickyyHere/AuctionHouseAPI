using AuctionHouseAPI.Application.DTOs.Read;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Auth.Queries
{
    public record GetAuthTokenQuery(LoginDTO LoginDTO) : IRequest<string>;
}
