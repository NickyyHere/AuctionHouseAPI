using AuctionHouseAPI.Application.DTOs.Create;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Bids.Commands
{
    public record CreateBidCommand(CreateBidDTO CreateBidDTO, int userId) : IRequest;
}
