using AuctionHouseAPI.Application.CQRS.Features.Auctions.Commands;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Shared.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuctionHouseAPI.Application.CQRS.Features.Auctions.Handlers
{
    public class UpdateAuctionOptionsHandler : IRequestHandler<UpdateAuctionOptionsCommand>
    {
        private readonly IAuctionService _auctionService;
        private readonly IAuctionRepository _auctionRepository;
        private readonly ILogger<UpdateAuctionOptionsHandler> _logger;
        public UpdateAuctionOptionsHandler(IAuctionService auctionService, IAuctionRepository auctionRepository, ILogger<UpdateAuctionOptionsHandler> logger)
        {
            _auctionService = auctionService;
            _auctionRepository = auctionRepository;
            _logger = logger;
        }

        public async Task Handle(UpdateAuctionOptionsCommand request, CancellationToken cancellationToken)
        {
            var auction = await _auctionRepository.GetByIdAsync(request.auctionId)
                ?? throw new EntityDoesNotExistException($"Auction with given id ({request.auctionId}) does not exist");
            await _auctionService.UpdateAuctionOptionsAsync(auction, request.UpdateAuctionOptionsDTO, request.userId);
            _logger.LogInformation("Auction {AuctionId} options has been updated", auction.Id);
        }
    }
}
