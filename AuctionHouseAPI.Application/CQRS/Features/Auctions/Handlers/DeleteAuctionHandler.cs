using AuctionHouseAPI.Application.CQRS.Features.Auctions.Commands;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Shared.Exceptions;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Auctions.Handlers
{
    public class DeleteAuctionHandler : IRequestHandler<DeleteAuctionCommand>
    {
        private readonly IAuctionService _auctionService;
        private readonly IAuctionRepository _auctionRepository;
        public DeleteAuctionHandler(IAuctionService auctionService, IAuctionRepository auctionRepository)
        {
            _auctionService = auctionService;
            _auctionRepository = auctionRepository;
        }

        public async Task Handle(DeleteAuctionCommand request, CancellationToken cancellationToken)
        {
            var auction = await _auctionRepository.GetByIdAsync(request.auctionId)
                ?? throw new EntityDoesNotExistException($"Auction with given id ({request.auctionId}) does not exist");
            await _auctionService.DeleteAuctionAsync(auction, request.userId);
        }
    }
}
