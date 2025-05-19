using AuctionHouseAPI.Models;

namespace AuctionHouseAPI.Repositories.interfaces
{
    public interface IAuctionRepository
    {
        public Task CreateAuction(Auction auction);
        public Task UpdateAuction();
        public Task DeleteAuction(Auction auction);
        public Task<Auction> GetAuctionById(int id);
        public Task<List<Auction>> GetAuctions();
        public Task<List<Auction>> GetUserAuctions(int userId);
        public Task<AuctionItem> GetAuctionItem(int id);
        public Task<List<AuctionItem>> GetAuctionItems();
        public Task<AuctionOptions> GetAuctionOptions(int id);
        public Task<List<Auction>> GetAuctionsByCategoryId(int categoryId);
        public Task<List<Auction>> GetAuctionsByTag(int tagId);
    }
}
