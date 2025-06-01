using AuctionHouseAPI.Application.CQRS.Features.Auctions.Queries;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Auctions.Handlers
{
    public class GetAllActiveAuctionsHandler : IRequestHandler<GetAllActiveAuctionsQuery, List<AuctionDTO>>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IMapper _mapper;
        public GetAllActiveAuctionsHandler(IAuctionRepository auctionRepository, IMapper mapper)
        {
            _auctionRepository = auctionRepository;
            _mapper = mapper;
        }

        public async Task<List<AuctionDTO>> Handle(GetAllActiveAuctionsQuery request, CancellationToken cancellationToken)
        {
            var auctions = await _auctionRepository.GetActiveAsync();
            return _mapper.Map<List<AuctionDTO>>(auctions);
        }
    }
}
