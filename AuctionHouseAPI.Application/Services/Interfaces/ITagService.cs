using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Application.Services.Interfaces
{
    public interface ITagService
    {
        public Task<int> CreateTag(Tag tag);
        public Task<List<Tag>> EnsureTagsExistAsync(List<string> tagNames);
    }
}
