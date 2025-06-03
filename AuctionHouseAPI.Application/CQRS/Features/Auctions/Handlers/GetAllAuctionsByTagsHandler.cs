using AuctionHouseAPI.Application.CQRS.Features.Auctions.Queries;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Auctions.Handlers
{
    public class GetAllAuctionsByTagsHandler : IRequestHandler<GetAllAuctionsByTagsQuery, List<AuctionDTO>>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IMapper _mapper;
        public GetAllAuctionsByTagsHandler(IAuctionRepository auctionRepository, IMapper mapper)
        {
            _auctionRepository = auctionRepository;
            _mapper = mapper;
        }

        public async Task<List<AuctionDTO>> Handle(GetAllAuctionsByTagsQuery request, CancellationToken cancellationToken)
        {
            var auctions = await Task
                .WhenAll(request.tags
                    .Select(t => _auctionRepository.GetByTagAsync(t)));
            return auctions
                .SelectMany(a => a)
                .Select(a => _mapper.Map<AuctionDTO>(a))
                .DistinctBy(a => a.Id)
                .ToList();
        }
    }
}
