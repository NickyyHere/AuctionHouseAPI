using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Domain.Repositories.interfaces
{
    public interface ITagRepository : IBaseRepository
    {
        public Task CreateTag(Tag tag);
        public void DeleteTag(Tag tag);
        public Task<List<Tag>> GetTags();
        public Task<Tag> GetTagByName(string name);
    }
}
