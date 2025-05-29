using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouseAPI.Domain.EFCore.Repositories
{
    public class EFAuctionRepository : EFBaseRepository<Auction>, IAuctionRepository
    {
        public EFAuctionRepository(AppDbContext context) : base(context) {}

        public override async Task<int> CreateAsync(Auction entity)
        {
            await _context.Auctions.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

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

        public async Task UpdateAuctionItemAsync(AuctionItem item)
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAuctionOptionsAsync(AuctionOptions options)
        {
            await _context.SaveChangesAsync();
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
