using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Domain.Interfaces
{
    public interface IAuctionRepository : IBaseRepository<Auction>, ITransactionRepository
    {
        public Task<IEnumerable<Auction>> GetByUserAsync(int userId);
        public Task<IEnumerable<Auction>> GetByCategoryIdAsync(int categoryId);
        public Task<IEnumerable<Auction>> GetByTagsAsync(string tag);
        public Task UpdateAuctionItemAsync(AuctionItem item);
        public Task UpdateAuctionOptionsAsync(AuctionOptions options);
    }
}
