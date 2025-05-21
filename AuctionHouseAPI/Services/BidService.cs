using AuctionHouseAPI.DTOs.Create;
using AuctionHouseAPI.DTOs.Read;
using AuctionHouseAPI.DTOs.Update;
using AuctionHouseAPI.Exceptions;
using AuctionHouseAPI.Mappers;
using AuctionHouseAPI.Models;
using AuctionHouseAPI.Repositories.interfaces;
using AuctionHouseAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace AuctionHouseAPI.Services
{
    public class BidService : IBidService
    {
        private readonly IBidRepository _bidRepository;
        private readonly IMapper<BidDTO, CreateBidDTO, Bid> _mapper;
        private readonly IAuctionService _auctionService;
        public BidService(IBidRepository bidRepository, IMapper<BidDTO, CreateBidDTO, Bid> mapper, IAuctionService auctionService)
        {
            _bidRepository = bidRepository;
            _mapper = mapper;
            _auctionService = auctionService;
        }

        public async Task CreateBid(CreateBidDTO createBidDTO, int userId)
        {
            var auctionOptions = await _auctionService.GetAuctionOptions(createBidDTO.AuctionId);
            if(!auctionOptions.IsActive)
            {
                throw new InactiveAuctionException($"Can't place bid on inactive auction");
            }
            if(auctionOptions.AllowBuyItNow && createBidDTO.Amount >= auctionOptions.BuyItNowPrice)
            {
                auctionOptions.IsActive = false;
                auctionOptions.FinishDateTime = DateTime.Now;
            }
            else
            { 
                var auctionBids = await _bidRepository.GetAuctionBids(createBidDTO.AuctionId);
                var minimumRequired = auctionBids.Any() 
                    ? auctionBids.Max(b => b.Amount) + auctionOptions.MinimumOutbid 
                    : auctionOptions.StartingPrice;

                if(createBidDTO.Amount < minimumRequired)
                {
                    throw new MinimumOutbidException($"Minimum outbid is {auctionOptions.MinimumOutbid}, {minimumRequired} to reach the minimum.");
                }
            }
            var bid = _mapper.ToEntity(createBidDTO);
            bid.UserId = userId;
            await _bidRepository.CreateBid(bid);
        }

        public async Task<List<BidDTO>> GetAuctionBids(int auctionId)
        {
            var bids = _mapper.ToDTO(await _bidRepository.GetAuctionBids(auctionId));
            return bids;
        }

        public async Task<List<BidDTO>> GetUserBids(int userId)
        {
            var bids = _mapper.ToDTO(await _bidRepository.GetUserBids(userId));
            return bids;
        }

        public async Task<List<BidDTO>> GetUsersBidsByAuctionId(int userId, int auctionId)
        {
            var bids = _mapper.ToDTO(await _bidRepository.GetAuctionUserBids(userId, auctionId));
            return bids;
        }

        public async Task WithdrawFromAuction(int auctionId, int userId)
        {
            var auction = await _auctionService.GetAuctionById(auctionId);
            if (auction.Options.FinishDateTime < DateTime.Now)
            {
                throw new FinishedAuctionException();
            }
            var userAuctionBids = await _bidRepository.GetAuctionUserBids(userId, auctionId);
            foreach(var bid in userAuctionBids)
            {
                await _bidRepository.DeleteBid(bid);
            }
        }
    }
}
