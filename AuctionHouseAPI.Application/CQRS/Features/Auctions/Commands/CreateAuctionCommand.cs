using AuctionHouseAPI.Application.DTOs.Create;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Auctions.Commands
{
    public record CreateAuctionCommand(CreateAuctionDTO CreateAuctionDTO, int userId) : IRequest<int>;
}
