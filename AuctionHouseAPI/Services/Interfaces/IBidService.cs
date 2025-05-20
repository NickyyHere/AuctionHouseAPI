using AuctionHouseAPI.DTOs.Create;
using AuctionHouseAPI.DTOs.Read;
using AuctionHouseAPI.DTOs.Update;

namespace AuctionHouseAPI.Services.Interfaces
{
    public interface IBidService
    {
        public Task CreateBid(CreateBidDTO createBidDTO);
        public Task UpdateBid(UpdateBidDTO updateBidDTO, int auctionId);
        public Task WithdrawFromAuction(int auctionId);
        public Task<List<BidDTO>> GetUserBids(int userId);
        public Task<List<BidDTO>> GetAuctionBids(int auctionId);
        public Task<List<BidDTO>> GetUsersBidsByAuctionId(int userId, int auctionId);
    }
}
