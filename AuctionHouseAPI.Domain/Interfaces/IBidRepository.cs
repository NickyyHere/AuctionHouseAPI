using AuctionHouseAPI.Domain.Models;
using System.Runtime.CompilerServices;

namespace AuctionHouseAPI.Domain.Interfaces
{
    public interface IBidRepository :  IBaseRepository<Bid>, ITransactionRepository
    {
        public Task<IEnumerable<Bid>> GetByUserAsync(int userId);
        public Task<IEnumerable<Bid>> GetByAuctionAsync(int auctionId);
        public Task<IEnumerable<Bid>> GetByUserAndAuctionAsync(int userId, int auctionId);
        public Task<Bid?> GetHighestAuctionBidAsync(int auctionId);
    }
}
