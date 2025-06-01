using AuctionHouseAPI.Application.CQRS.Features.Bids.Queries;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Bids.Handlers
{
    public class GetAllAuctionBidsHandler : IRequestHandler<GetAllAuctionBidsQuery, List<BidDTO>>
    {
        private readonly IBidRepository _bidRepository;
        private readonly IMapper _mapper;
        public GetAllAuctionBidsHandler(IBidRepository bidRepository, IMapper mapper)
        {
            _bidRepository = bidRepository;
            _mapper = mapper;
        }
        public async Task<List<BidDTO>> Handle(GetAllAuctionBidsQuery request, CancellationToken cancellationToken)
        {
            var bids = await _bidRepository.GetByAuctionAsync(request.auctionId);
            return _mapper.Map<List<BidDTO>>(bids);
        }
    }
}
