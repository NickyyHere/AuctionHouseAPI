using AuctionHouseAPI.Application.CQRS.Features.Auctions.Commands;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuctionHouseAPI.Application.CQRS.Features.Auctions.Handlers
{
    public class CreateAuctionHandler : IRequestHandler<CreateAuctionCommand, int>
    {
        private readonly IAuctionService _auctionService;
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateAuctionHandler> _logger;
        public CreateAuctionHandler(IAuctionService auctionService, ITagService tagService, IMapper mapper, ILogger<CreateAuctionHandler> logger)
        {
            _auctionService = auctionService;
            _tagService = tagService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Handle(CreateAuctionCommand request, CancellationToken cancellationToken)
        {
            var auction = _mapper.Map<Auction>(request.CreateAuctionDTO);
            var tags = await _tagService.EnsureTagsExistAsync(request.CreateAuctionDTO.Item.CustomTags);
            _auctionService.AddTagsToAuction(tags, auction);
            auction.OwnerId = request.userId;
            var auctionId = await _auctionService.CreateAuctionAsync(auction);
            _logger.LogInformation("Auction {AuctionId} has been created", auctionId);
            return auctionId;
        }
    }
}
