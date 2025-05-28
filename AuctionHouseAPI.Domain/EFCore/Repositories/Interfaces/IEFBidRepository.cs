using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Domain.EFCore.Repositories.Interfaces
{
    public interface IEFBidRepository :  IEFCoreRepository<Bid>
    {
        public Task<IEnumerable<Bid>> GetByUserAsync(int userId);
        public Task<IEnumerable<Bid>> GetByAuctionAsync(int auctionId);
        public Task<IEnumerable<Bid>> GetByUserAndAuctionAsync(int userId, int auctionId);
    }
}
