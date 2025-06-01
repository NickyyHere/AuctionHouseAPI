using AuctionHouseAPI.Application.DTOs.Read;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseAPI.Application.CQRS.Features.Auctions.Queries
{
    public record GetAllAuctionsByTagsQuery(List<string> tags) : IRequest<List<AuctionDTO>>;
}
