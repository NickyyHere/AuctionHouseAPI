using AuctionHouseAPI.Application.CQRS.Features.Auctions.Queries;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Auctions.Handlers
{
    public class GetAuctionOptionsByIdHandler : IRequestHandler<GetAuctionOptionsByIdQuery, AuctionOptionsDTO>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IMapper _mapper;
        public GetAuctionOptionsByIdHandler(IAuctionRepository auctionRepository, IMapper mapper)
        {
            _auctionRepository = auctionRepository;
            _mapper = mapper;
        }

        public async Task<AuctionOptionsDTO> Handle(GetAuctionOptionsByIdQuery request, CancellationToken cancellationToken)
        {
            var auction = _mapper.Map<AuctionDTO>(await _auctionRepository.GetByIdAsync(request.auctionId));
            return auction.Options;
        }
    }
}
