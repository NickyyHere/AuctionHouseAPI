using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Domain.Repositories.interfaces;

namespace AuctionHouseAPI.Application.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task CreateTag(Tag tag)
        {
            await _tagRepository.CreateTag(tag);
        }

        public async Task<Tag> GetTagByName(string name)
        {
            return await _tagRepository.GetTagByName(name);
        }
    }
}
