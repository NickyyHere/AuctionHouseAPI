using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouseAPI.Domain.EFCore.Repositories
{
    public class EFBidRepository : EFBaseRepository<Bid>, IBidRepository
    {
        public EFBidRepository(AppDbContext context) : base(context) { }

        public override async Task<int> CreateAsync(Bid entity)
        {
            await _context.Bids.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

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

        public async Task<Bid?> GetHighestAuctionBidAsync(int auctionId)
        {
            var highestBid = await _context.Bids.MaxAsync(b => b.Amount);
            return await _context.Bids
                .Where(b => b.AuctionId == auctionId && b.Amount == highestBid)
                .FirstOrDefaultAsync();
        }
    }
}
