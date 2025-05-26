using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Domain.Repositories.interfaces
{
    public interface ITagRepository
    {
        public Task<Tag> CreateTag(Tag tag);
        public Task DeleteTag(Tag tag);
        public Task<List<Tag>> GetTags();
        public Task<Tag> GetTagByName(string name);
    }
}
