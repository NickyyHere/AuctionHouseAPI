using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Bids.Commands
{
    public record DeleteAllUserBidsFromAuctionCommand(int userId, int auctionId) : IRequest;
}
