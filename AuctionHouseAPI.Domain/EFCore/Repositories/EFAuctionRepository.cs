using AuctionHouseAPI.Domain.EFCore.Repositories.Interfaces;
using AuctionHouseAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouseAPI.Domain.EFCore.Repositories
{
    public class EFAuctionRepository : EFBaseRepository<Auction>, IEFAuctionRepository
    {
        public EFAuctionRepository(AppDbContext context) : base(context) {}

        public async Task<IEnumerable<Auction>> GetByCategoryIdAsync(int categoryId)
        {
            return await GetAllAuctionsWithDetails()
                .Where(a => a.Item!.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Auction>> GetByTagsAsync(string tag)
        {
            return await GetAllAuctionsWithDetails()
                .Where(a => a.Item!.Tags.Any(it => it.Tag!.Name == tag))
                .ToListAsync();
        }

        public async Task<IEnumerable<Auction>> GetByUserAsync(int userId)
        {
            return await GetAllAuctionsWithDetails()
                .Where(a => a.OwnerId == userId)
                .ToListAsync();
        }

        private IQueryable<Auction> GetAllAuctionsWithDetails()
        {
            return _context.Auctions
                .Include(a => a.Item)
                    .ThenInclude(i => i!.Tags)
                        .ThenInclude(t => t.Tag)
                .AsNoTracking();
        }
    }
}
