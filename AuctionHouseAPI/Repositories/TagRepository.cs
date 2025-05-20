using AuctionHouseAPI.Exceptions;
using AuctionHouseAPI.Models;
using AuctionHouseAPI.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouseAPI.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly AppDbContext _context;
        public TagRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Tag> CreateTag(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task DeleteTag(Tag tag)
        {
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }

        public async Task<Tag> GetTagByName(string name)
        {
            return await _context.Tags.FirstOrDefaultAsync(t => t.Name == name) ?? throw new EntityDoesNotExistException($"Tag with name {name} does not exist in database");
        }

        public async Task<List<Tag>> GetTags()
        {
            return await _context.Tags.ToListAsync();
        }
    }
}
