using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Domain.Repositories.interfaces
{
    public interface IAuctionRepository : IBaseRepository
    {
        public Task CreateAuction(Auction auction);
        public void DeleteAuction(Auction auction);
        public Task<Auction> GetAuctionById(int id);
        public Task<List<Auction>> GetAuctions();
        public Task<List<Auction>> GetUserAuctions(int userId);
        public Task<List<Auction>> GetAuctionsByCategoryId(int categoryId);
        public Task<List<Auction>> GetAuctionsByTag(string tag);
    }
}
