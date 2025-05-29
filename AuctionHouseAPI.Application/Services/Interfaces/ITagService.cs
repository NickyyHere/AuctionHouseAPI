using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Application.Services.Interfaces
{
    public interface ITagService
    {
        public Task<int> CreateTag(Tag tag);
        public Task<Tag?> GetTagById(int id);
        public Task<Tag> GetTagByName(string name);
    }
}
