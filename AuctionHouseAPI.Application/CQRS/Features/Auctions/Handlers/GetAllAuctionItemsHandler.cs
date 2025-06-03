using AuctionHouseAPI.Application.CQRS.Features.Auctions.Queries;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Auctions.Handlers
{
    class GetAllAuctionItemsHandler : IRequestHandler<GetAllAuctionItemsQuery, List<AuctionItemDTO>>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IMapper _mapper;
        public GetAllAuctionItemsHandler(IAuctionRepository auctionRepository, IMapper mapper)
        {
            _auctionRepository = auctionRepository;
            _mapper = mapper;
        }

        public async Task<List<AuctionItemDTO>> Handle(GetAllAuctionItemsQuery request, CancellationToken cancellationToken)
        {
            var auctions = _mapper.Map<List<AuctionDTO>>(await _auctionRepository.GetAllAsync());
            var items = auctions.Select(a => a.Item).ToList();
            return items;
        }
    }
}
