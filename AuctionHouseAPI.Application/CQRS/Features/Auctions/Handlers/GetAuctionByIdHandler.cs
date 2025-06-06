using AuctionHouseAPI.Application.CQRS.Features.Auctions.Queries;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Shared.Exceptions;
using AutoMapper;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Auctions.Handlers
{
    public class GetAuctionByIdHandler : IRequestHandler<GetAuctionByIdQuery, AuctionDTO>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IMapper _mapper;
        public GetAuctionByIdHandler(IAuctionRepository auctionRepository, IMapper mapper)
        {
            _auctionRepository = auctionRepository;
            _mapper = mapper;
        }

        public async Task<AuctionDTO> Handle(GetAuctionByIdQuery request, CancellationToken cancellationToken)
        {
            var auction = await _auctionRepository.GetByIdAsync(request.id)
                ?? throw new EntityDoesNotExistException($"Auction with given id ({request.id}) does not exist");
            return _mapper.Map<AuctionDTO>(auction);
        }
    }
}
