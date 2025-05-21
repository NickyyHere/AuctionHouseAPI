using AuctionHouseAPI.Models;

namespace AuctionHouseAPI.Repositories.interfaces
{
    public interface IBidRepository
    {
        public Task CreateBid(Bid bid);
        public Task DeleteBid(Bid bid);
        public Task<List<Bid>> GetUserBids(int userId);
        public Task<List<Bid>> GetAuctionBids(int auctionId);
        public Task<List<Bid>> GetAuctionUserBids(int userId, int auctionId);
    }
}
