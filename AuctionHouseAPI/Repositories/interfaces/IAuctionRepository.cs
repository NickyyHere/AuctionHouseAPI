using AuctionHouseAPI.Models;

namespace AuctionHouseAPI.Repositories.interfaces
{
    public interface IAuctionRepository
    {
        public Task<int> CreateAuction(Auction auction);
        public Task UpdateAuction();
        public Task DeleteAuction(Auction auction);
        public Task<Auction> GetAuctionById(int id);
        public Task<List<Auction>> GetAuctions();
        public Task<List<Auction>> GetUserAuctions(int userId);
        public Task<List<Auction>> GetAuctionsByCategoryId(int categoryId);
        public Task<List<Auction>> GetAuctionsByTag(string tag);
    }
}
