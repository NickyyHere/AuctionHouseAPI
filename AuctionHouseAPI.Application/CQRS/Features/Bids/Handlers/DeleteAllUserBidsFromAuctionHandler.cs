using AuctionHouseAPI.Application.CQRS.Features.Bids.Commands;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Shared.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuctionHouseAPI.Application.CQRS.Features.Bids.Handlers
{
    public class DeleteAllUserBidsFromAuctionHandler : IRequestHandler<DeleteAllUserBidsFromAuctionCommand>
    {
        private readonly IBidService _bidService;
        private readonly IAuctionRepository _auctionRepository;
        private readonly ILogger<DeleteAllUserBidsFromAuctionHandler> _logger;
        public DeleteAllUserBidsFromAuctionHandler(IAuctionRepository auctionRepository, IBidService bidService, ILogger<DeleteAllUserBidsFromAuctionHandler> logger)
        {
            _bidService = bidService;
            _auctionRepository = auctionRepository;
            _logger = logger;
        }
        public async Task Handle(DeleteAllUserBidsFromAuctionCommand request, CancellationToken cancellationToken)
        {
            var auction = await _auctionRepository.GetByIdAsync(request.auctionId)
                ?? throw new EntityDoesNotExistException($"Auction with given id ({request.auctionId}) does not exist");
            await _bidService.WithdrawFromAuctionAsync(auction, request.userId);
            _logger.LogInformation("User {UserId} withdrew from Auction {AuctionId}", request.userId, request.auctionId);
        }
    }
}
