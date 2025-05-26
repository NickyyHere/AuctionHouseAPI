using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Application.Services.Interfaces
{
    public interface ITagService
    {
        public Task CreateTag(Tag tag);
        public Task<Tag> GetTagByName(string name);
    }
}
