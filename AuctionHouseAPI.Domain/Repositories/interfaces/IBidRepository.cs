using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Domain.Repositories.interfaces
{
    public interface IBidRepository :  IBaseRepository
    {
        public Task CreateBid(Bid bid);
        public void DeleteBid(Bid bid);
        public Task<List<Bid>> GetUserBids(int userId);
        public Task<List<Bid>> GetAuctionBids(int auctionId);
        public Task<List<Bid>> GetAuctionUserBids(int userId, int auctionId);
    }
}
