using AuctionHouseAPI.Application.DTOs.Read;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Auctions.Queries
{
    public record GetAllActiveAuctionsQuery() : IRequest<List<AuctionDTO>>;
}
