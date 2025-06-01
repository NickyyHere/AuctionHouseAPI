using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Auctions.Commands
{
    public record DeleteAuctionCommand(int auctionId, int userId) : IRequest;
}
