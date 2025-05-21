using AuctionHouseAPI.Models;
using AuctionHouseAPI.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouseAPI.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly AppDbContext _context;
        public BidRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateBid(Bid bid)
        {
            await _context.Bids.AddAsync(bid);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBid(Bid bid)
        {
            _context.Bids.Remove(bid);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Bid>> GetAuctionBids(int auctionId)
        {
            return await _context.Bids
                .Where(b => b.AuctionId == auctionId)
                .ToListAsync();
        }

        public async Task<List<Bid>> GetAuctionUserBids(int userId, int auctionId)
        {
            return await _context.Bids
                .Where(b => b.UserId == userId && b.AuctionId == auctionId)
                .ToListAsync();
        }

        public async Task<List<Bid>> GetUserBids(int userId)
        {
            return await _context.Bids
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }
    }
}
