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

        public async Task<IEnumerable<Auction>> GetByTagAsync(string tag)
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
            await SaveTrackedEntites();
        }

        public async Task UpdateAuctionOptionsAsync(AuctionOptions options)
        {
            await SaveTrackedEntites();
        }

        public override async Task<IEnumerable<Auction>> GetAllAsync()
        {
            return await GetAllAuctionsWithDetails().ToListAsync();
        }
        public override async Task<Auction?> GetByIdAsync(int id)
        {
            return await _context.Auctions
                .Include(a => a.Item)
                    .ThenInclude(i => i!.Tags)
                        .ThenInclude(t => t.Tag)
                .Include(a => a.Options)
                .Include(a => a.Owner)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<IEnumerable<Auction>> GetActiveAsync()
        {
            return await GetAllAuctionsWithDetails()
                .Where(a => a.Options!.StartDateTime <= DateTime.UtcNow && a.Options.FinishDateTime >= DateTime.UtcNow)
                .ToListAsync();
        }
        private IQueryable<Auction> GetAllAuctionsWithDetails()
        {
            return _context.Auctions
                .Include(a => a.Item)
                    .ThenInclude(i => i!.Tags)
                        .ThenInclude(t => t.Tag)
                .Include(a => a.Options)
                .Include(a => a.Owner)
                .AsNoTracking();
        }
    
        private async Task SaveTrackedEntites() => await _context.SaveChangesAsync();
    }
}
