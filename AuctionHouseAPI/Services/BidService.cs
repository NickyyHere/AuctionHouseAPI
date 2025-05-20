using AuctionHouseAPI.DTOs.Create;
using AuctionHouseAPI.DTOs.Read;
using AuctionHouseAPI.DTOs.Update;
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
        public BidService(IBidRepository bidRepository, IMapper<BidDTO, CreateBidDTO, Bid> mapper)
        {
            _bidRepository = bidRepository;
            _mapper = mapper;
        }

        public async Task CreateBid(CreateBidDTO createBidDTO)
        {
            var bid = _mapper.ToEntity(createBidDTO);
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

        public async Task UpdateBid(UpdateBidDTO updateBidDTO, int auctionId)
        {
            // Checking for user missing
            var bids = await _bidRepository.GetAuctionBids(auctionId);
            var highestBid = bids.OrderByDescending(b => b.Amount).ToList()[0];
            highestBid.Amount = updateBidDTO.Amount;
        }

        public Task WithdrawFromAuction(int auctionId)
        {
            // Checking for user missing
            throw new NotImplementedException();
        }
    }
}
