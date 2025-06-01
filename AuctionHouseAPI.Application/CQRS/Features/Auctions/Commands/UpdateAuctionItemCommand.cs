using AuctionHouseAPI.Application.DTOs.Update;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Auctions.Commands
{
    public record UpdateAuctionItemCommand(UpdateAuctionItemDTO UpdateAuctionItemDTO, int auctionId, int userId) : IRequest;
}
