using AuctionHouseAPI.Application.CQRS.Features.Bids.Commands;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Shared.Exceptions;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Bids.Handlers
{
    public class DeleteAllUserBidsFromAuctionHandler : IRequestHandler<DeleteAllUserBidsFromAuctionCommand>
    {
        private readonly IBidService _bidService;
        private readonly IAuctionRepository _auctionRepository;
        public DeleteAllUserBidsFromAuctionHandler(IAuctionRepository auctionRepository, IBidService bidService)
        {
            _bidService = bidService;
            _auctionRepository = auctionRepository;
        }
        public async Task Handle(DeleteAllUserBidsFromAuctionCommand request, CancellationToken cancellationToken)
        {
            var auction = await _auctionRepository.GetByIdAsync(request.auctionId)
                ?? throw new EntityDoesNotExistException($"Auction with given id ({request.auctionId}) does not exist");
            await _bidService.WithdrawFromAuctionAsync(auction, request.userId);
        }
    }
}
