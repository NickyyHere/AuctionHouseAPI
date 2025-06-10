using AuctionHouseAPI.Application.DTOs.Read;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Auctions.Queries
{
    public record GetAllAuctionsByTagsQuery(List<string> tags) : IRequest<List<AuctionDTO>>;
}
