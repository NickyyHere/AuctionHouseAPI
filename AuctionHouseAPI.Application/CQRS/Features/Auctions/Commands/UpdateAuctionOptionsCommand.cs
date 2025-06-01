using AuctionHouseAPI.Application.DTOs.Update;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Auctions.Commands
{
    public record UpdateAuctionOptionsCommand(UpdateAuctionOptionsDTO UpdateAuctionOptionsDTO, int auctionId, int userId) : IRequest;
}
