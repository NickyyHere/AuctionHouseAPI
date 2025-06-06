using AuctionHouseAPI.Application.CQRS.Features.Auctions.Commands;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AutoMapper;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Auctions.Handlers
{
    public class CreateAuctionHandler : IRequestHandler<CreateAuctionCommand, int>
    {
        private readonly IAuctionService _auctionService;
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;
        public CreateAuctionHandler(IAuctionService auctionService, ITagService tagService, IMapper mapper)
        {
            _auctionService = auctionService;
            _tagService = tagService;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateAuctionCommand request, CancellationToken cancellationToken)
        {
            var auction = _mapper.Map<Auction>(request.CreateAuctionDTO);
            var tags = await _tagService.EnsureTagsExistAsync(request.CreateAuctionDTO.Item.CustomTags);
            _auctionService.AddTagsToAuction(tags, auction);
            auction.OwnerId = request.userId;
            var auctionId = await _auctionService.CreateAuctionAsync(auction);
            return auctionId;
        }
    }
}
