using AuctionHouseAPI.Application.DTOs.Read;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Bids.Queries
{
    public record GetAllUserBidsQuery(int userId) : IRequest<List<BidDTO>>;
}
