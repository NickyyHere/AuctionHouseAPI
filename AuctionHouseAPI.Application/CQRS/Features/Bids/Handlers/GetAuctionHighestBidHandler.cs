using AuctionHouseAPI.Application.CQRS.Features.Bids.Queries;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Bids.Handlers
{
    public class GetAuctionHighestBidHandler : IRequestHandler<GetAuctionHighestBidQuery, BidDTO>
    {
        private readonly IBidRepository _bidRepository;
        private readonly IMapper _mapper;
        public GetAuctionHighestBidHandler(IBidRepository bidRepository, IMapper mapper)
        {
            _bidRepository = bidRepository;
            _mapper = mapper;
        }
        public async Task<BidDTO> Handle(GetAuctionHighestBidQuery request, CancellationToken cancellationToken)
        {
            var bid = await _bidRepository.GetHighestAuctionBidAsync(request.auctionId);
            return _mapper.Map<BidDTO>(bid);
        }
    }
}
