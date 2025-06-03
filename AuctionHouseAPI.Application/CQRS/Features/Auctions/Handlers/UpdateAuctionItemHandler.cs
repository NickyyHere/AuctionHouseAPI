using AuctionHouseAPI.Application.CQRS.Features.Auctions.Commands;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Shared.Exceptions;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Auctions.Handlers
{
    public class UpdateAuctionItemHandler : IRequestHandler<UpdateAuctionItemCommand>
    {
        private readonly IAuctionService _auctionService;
        private readonly IAuctionRepository _auctionRepository;
        private readonly ITagService _tagService;
        public UpdateAuctionItemHandler(IAuctionService auctionService, IAuctionRepository auctionRepository, ITagService tagService)
        {
            _auctionService = auctionService;
            _auctionRepository = auctionRepository;
            _tagService = tagService;
        }

        public async Task Handle(UpdateAuctionItemCommand request, CancellationToken cancellationToken)
        {
            var auction = await _auctionRepository.GetByIdAsync(request.auctionId)
                ?? throw new EntityDoesNotExistException($"Auction with given id ({request.auctionId}) does not exist");
            var tags = await _tagService.EnsureTagsExistAsync(request.UpdateAuctionItemDTO.CustomTags);
            _auctionService.AddTagsToAuction(tags, auction);
            await _auctionService.UpdateAuctionItemAsync(auction, request.UpdateAuctionItemDTO, request.userId);
        }
    }
}
