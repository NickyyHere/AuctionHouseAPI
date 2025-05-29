using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Shared.Exceptions;

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
            await _tagRepository.CreateAsync(tag);
        }

        public async Task<Tag> GetTagByName(string name)
        {
            return await _tagRepository.GetByNameAsync(name) ?? throw new EntityDoesNotExistException($"Tag with given name ({name}) does not exist");
        }
    }
}
