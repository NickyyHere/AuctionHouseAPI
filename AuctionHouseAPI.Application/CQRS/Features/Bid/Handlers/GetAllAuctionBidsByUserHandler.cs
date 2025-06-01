using AuctionHouseAPI.Application.CQRS.Features.Bids.Queries;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Bids.Handlers
{
    public class GetAllAuctionBidsByUserHandler : IRequestHandler<GetAllAuctionBidsByUserQuery, List<BidDTO>>
    {
        private readonly IBidRepository _bidRepository;
        private readonly IMapper _mapper;
        public GetAllAuctionBidsByUserHandler(IBidRepository bidRepository, IMapper mapper)
        {
            _bidRepository = bidRepository;
            _mapper = mapper;
        }
        public async Task<List<BidDTO>> Handle(GetAllAuctionBidsByUserQuery request, CancellationToken cancellationToken)
        {
            var bids = await _bidRepository.GetByUserAndAuctionAsync(request.userId, request.auctionId);
            return _mapper.Map<List<BidDTO>>(bids);
        }
    }
}
