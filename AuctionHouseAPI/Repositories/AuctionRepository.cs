using AuctionHouseAPI.Exceptions;
using AuctionHouseAPI.Models;
using AuctionHouseAPI.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouseAPI.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly AppDbContext _context;
        public AuctionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateAuction(Auction auction)
        {
            await _context.Auctions.AddAsync(auction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAuction(Auction auction)
        {
            _context.Auctions.Remove(auction);
            await _context.SaveChangesAsync();
        }

        public async Task<Auction> GetAuctionById(int id)
        {
            var auction = await _context.Auctions
                .Include(a => a.Item)
                .Include(a => a.Options)
                .FirstOrDefaultAsync(a => a.Id == id)
                ?? throw new EntityDoesNotExistException($"Auction with given id ({id}) does not exist in database");
            return auction;
        }

        public async Task<AuctionItem> GetAuctionItem(int id)
        {
            return await _context.AuctionItems.FindAsync(id) ?? throw new EntityDoesNotExistException($"Auction item with given id ({id}) does not exist in database");
        }

        public async Task<List<AuctionItem>> GetAuctionItems()
        {
            return await _context.AuctionItems.ToListAsync();
        }

        public async Task<AuctionOptions> GetAuctionOptions(int id)
        {
            return await _context.AuctionOptions.FindAsync(id) ?? throw new EntityDoesNotExistException($"Auction with given id ({id}) does not exist in database");
        }

        public async Task<List<Auction>> GetAuctions()
        {
            return await _context.Auctions
                .Include (a => a.Item)
                .Include(a => a.Options)
                .ToListAsync();
        }

        public async Task<List<Auction>> GetAuctionsByCategoryId(int categoryId)
        {
            return await _context.Auctions
                .Include(a => a.Item)
                .Include(a => a.Options)
                .Where(a => a.Item.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<List<Auction>> GetAuctionsByTag(int tagId)
        {
            return await _context.Auctions
                .Include(a => a.Item)
                    .ThenInclude(ai => ai.Tags)
                        .ThenInclude(ait => ait.Tag)
                .Include(a => a.Options)
                .Where(a => a.Item.Tags.Any(ait => ait.TagId == tagId))
                .ToListAsync();
        }

        public async Task<List<Auction>> GetUserAuctions(int userId)
        {
            return await _context.Auctions
                .Include(a => a.Item)
                .Include(a => a.Options)
                .Where(a => a.OwnerId == userId)
                .ToListAsync();
        }

        public async Task UpdateAuction()
        {
            await _context.SaveChangesAsync();
        }
    }
}
