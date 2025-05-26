using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Domain.Repositories.interfaces;
using AuctionHouseAPI.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouseAPI.Domain.Repositories
{
    public class TagRepository : BaseRepository, ITagRepository
    {
        public TagRepository(AppDbContext context) : base(context) { }
        public async Task CreateTag(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
        }

        public void DeleteTag(Tag tag)
        {
            _context.Tags.Remove(tag);
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
