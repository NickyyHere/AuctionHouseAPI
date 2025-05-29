using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Shared.Exceptions;
using AutoMapper;

namespace AuctionHouseAPI.Application.Services
{
    public class BidService : IBidService
    {
        private readonly IBidRepository _bidRepository;
        private readonly IMapper _mapper;
        private readonly IAuctionService _auctionService;
        public BidService(IBidRepository bidRepository, IMapper mapper, IAuctionService auctionService)
        {
            _bidRepository = bidRepository;
            _mapper = mapper;
            _auctionService = auctionService;
        }

        public async Task CreateBid(CreateBidDTO createBidDTO, int userId)
        {
            var auctionOptions = await _auctionService.GetAuctionOptions(createBidDTO.AuctionId);
            await _bidRepository.BeginTransactionAsync();
            try
            {
                if (!auctionOptions.IsActive)
                {
                    throw new InactiveAuctionException($"Can't place bid on inactive auction");
                }
                else
                {
                    var auctionBids = await _bidRepository.GetByAuctionAsync(createBidDTO.AuctionId);
                    var minimumRequired = auctionBids.Any()
                        ? auctionBids.Max(b => b.Amount) + auctionOptions.MinimumOutbid
                        : auctionOptions.StartingPrice;

                    if (createBidDTO.Amount < minimumRequired)
                    {
                        throw new MinimumOutbidException($"Minimum outbid is {auctionOptions.MinimumOutbid}, {minimumRequired} to reach the minimum.");
                    }
                }
                var bid = _mapper.Map<Bid>(createBidDTO);
                bid.UserId = userId;
                await _bidRepository.CreateAsync(bid);
                await _bidRepository.CommitTransactionAsync();
            }
            catch
            {
                await _bidRepository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<List<BidDTO>> GetAuctionBids(int auctionId)
        {
            var bids = _mapper.Map<List<BidDTO>>(await _bidRepository.GetByAuctionAsync(auctionId));
            return bids;
        }

        public async Task<List<BidDTO>> GetUserBids(int userId)
        {
            var bids = _mapper.Map<List<BidDTO>>(await _bidRepository.GetByUserAsync(userId));
            return bids;
        }

        public async Task<List<BidDTO>> GetUsersBidsByAuctionId(int userId, int auctionId)
        {
            var bids = _mapper.Map<List<BidDTO>>(await _bidRepository.GetByUserAndAuctionAsync(userId, auctionId));
            return bids;
        }

        public async Task WithdrawFromAuction(int auctionId, int userId)
        {
            await _bidRepository.BeginTransactionAsync();
            try
            {
                var auction = await _auctionService.GetAuctionById(auctionId);
                if (auction.Options.FinishDateTime < DateTime.Now)
                {
                    throw new FinishedAuctionException();
                }
                var userAuctionBids = await _bidRepository.GetByUserAndAuctionAsync(userId, auctionId);
                foreach (var bid in userAuctionBids)
                {
                    await _bidRepository.DeleteAsync(bid);
                }
                await _bidRepository.CommitTransactionAsync();
            }
            catch
            {
                await _bidRepository.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
