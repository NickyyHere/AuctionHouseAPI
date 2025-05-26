using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Domain.Repositories.interfaces;
using AuctionHouseAPI.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouseAPI.Domain.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly AppDbContext _context;
        public AuctionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> CreateAuction(Auction auction)
        {
            await _context.Auctions.AddAsync(auction);
            await _context.SaveChangesAsync();
            return auction.Id;
        }

        public async Task DeleteAuction(Auction auction)
        {
            _context.Auctions.Remove(auction);
            await _context.SaveChangesAsync();
        }

        public async Task<Auction> GetAuctionById(int id)
        {
            var auction = await GetAllAuctionsWithDetails()
                .FirstOrDefaultAsync(a => a.Id == id)
                ?? throw new EntityDoesNotExistException($"Auction with given id ({id}) does not exist in database");
            return auction;
        }

        public async Task<List<Auction>> GetAuctions()
        {
            return await GetAllAuctionsWithDetails()
                .ToListAsync();
        }

        public async Task<List<Auction>> GetAuctionsByCategoryId(int categoryId)
        {
            return await GetAllAuctionsWithDetails()
                .Where(a => a.Item!.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<List<Auction>> GetAuctionsByTag(string tag)
        {
            #pragma warning disable CS8602
            return await GetAllAuctionsWithDetails()
                .Where(a => a.Item.Tags.Any(ait => ait.Tag.Name == tag))
                .ToListAsync();
        }

        public async Task<List<Auction>> GetUserAuctions(int userId)
        {
            return await GetAllAuctionsWithDetails()
                .Where(a => a.OwnerId == userId)
                .ToListAsync();
        }

        public async Task UpdateAuction()
        {
            await _context.SaveChangesAsync();
        }

        private IQueryable<Auction> GetAllAuctionsWithDetails()
        {
            return _context.Auctions
                .Include(a => a.Item)
                    .ThenInclude(i => i.Tags)
                        .ThenInclude(ait => ait.Tag)
                .Include(a => a.Options)
                .Include(a => a.Owner);
        }
    }
}
