using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Domain.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouseAPI.Domain.Repositories
{
    public class BidRepository : BaseRepository, IBidRepository
    {
        public BidRepository(AppDbContext context) : base(context) { }
        public async Task CreateBid(Bid bid)
        {
            await _context.Bids.AddAsync(bid);
        }

        public void DeleteBid(Bid bid)
        {
            _context.Bids.Remove(bid);
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
