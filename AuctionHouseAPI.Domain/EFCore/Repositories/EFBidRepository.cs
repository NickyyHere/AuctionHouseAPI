using AuctionHouseAPI.Domain.EFCore.Repositories.Interfaces;
using AuctionHouseAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouseAPI.Domain.EFCore.Repositories
{
    public class EFBidRepository : EFBaseRepository<Bid>, IEFBidRepository
    {
        public EFBidRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Bid>> GetByAuctionAsync(int auctionId)
        {
            return await _context.Bids
                .Where(b => b.AuctionId == auctionId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Bid>> GetByUserAndAuctionAsync(int userId, int auctionId)
        {
            return await _context.Bids
                .Where(b => b.UserId == userId && b.AuctionId == auctionId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Bid>> GetByUserAsync(int userId)
        {
            return await _context.Bids
                .Where(b => b.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
