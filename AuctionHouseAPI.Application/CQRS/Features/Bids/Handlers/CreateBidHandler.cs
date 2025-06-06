using AuctionHouseAPI.Application.CQRS.Features.Bids.Commands;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Shared.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuctionHouseAPI.Application.CQRS.Features.Bids.Handlers
{
    public class CreateBidHandler : IRequestHandler<CreateBidCommand>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IBidService _bidService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateBidHandler> _logger;
        public CreateBidHandler(IAuctionRepository auctionRepository, IBidService bidService, IMapper mapper, ILogger<CreateBidHandler> logger)
        {
            _auctionRepository = auctionRepository;
            _bidService = bidService;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task Handle(CreateBidCommand request, CancellationToken cancellationToken)
        {
            var auction = await _auctionRepository.GetByIdAsync(request.CreateBidDTO.AuctionId)
                ?? throw new EntityDoesNotExistException($"Auction with given id ({request.CreateBidDTO.AuctionId}) does not exist");
            var bid = _mapper.Map<Bid>(request.CreateBidDTO);
            await _bidService.CreateBidAsync(bid, auction, request.userId);
            _logger.LogInformation("User {UserId} placed a bid on Auction {AuctionId}, Bid amount: {Amount}", request.userId, request.CreateBidDTO.AuctionId, request.CreateBidDTO.Amount);
        }
    }
}
