using AuctionHouseAPI.Application.CQRS.Features.Bids.Queries;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Bids.Handlers
{
    public class GetAllUserBidsHandler : IRequestHandler<GetAllUserBidsQuery, List<BidDTO>>
    {
        private readonly IBidRepository _bidRepository;
        private readonly IMapper _mapper;
        public GetAllUserBidsHandler(IBidRepository bidRepository, IMapper mapper)
        {
            _bidRepository = bidRepository;
            _mapper = mapper;
        }
        public async Task<List<BidDTO>> Handle(GetAllUserBidsQuery request, CancellationToken cancellationToken)
        {
            var bids = await _bidRepository.GetByUserAsync(request.userId);
            return _mapper.Map<List<BidDTO>>(bids);
        }
    }
}
