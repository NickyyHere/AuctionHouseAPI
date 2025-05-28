using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Domain.EFCore.Repositories.Interfaces
{
    public interface IEFAuctionRepository : IEFCoreRepository<Auction>
    {
        public Task<IEnumerable<Auction>> GetByUserAsync(int userId);
        public Task<IEnumerable<Auction>> GetByCategoryIdAsync(int categoryId);
        public Task<IEnumerable<Auction>> GetByTagsAsync(string tag);
    }
}
