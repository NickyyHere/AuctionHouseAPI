using AuctionHouseAPI.Application.CQRS.Features.Auctions.Queries;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Auctions.Handlers
{
    class GetAllAuctionsHandler : IRequestHandler<GetAllAuctionsQuery, List<AuctionDTO>>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IMapper _mapper;
        public GetAllAuctionsHandler(IAuctionRepository auctionRepository, IMapper mapper)
        {
            _auctionRepository = auctionRepository;
            _mapper = mapper;
        }
        public async Task<List<AuctionDTO>> Handle(GetAllAuctionsQuery request, CancellationToken cancellationToken)
        {
            var auctions = await _auctionRepository.GetAllAsync();
            return _mapper.Map<List<AuctionDTO>>(auctions);
        }
    }
}
