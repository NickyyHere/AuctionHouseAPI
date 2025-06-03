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

        public async Task<int> CreateTag(Tag tag)
        {
            await _tagRepository.BeginTransactionAsync();
            try
            {
                var tagId = await _tagRepository.CreateAsync(tag);
                await _tagRepository.CommitTransactionAsync();
                return tagId;
            }
            catch
            {
                await _tagRepository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<Tag?> GetTagById(int id)
        {
            return await _tagRepository.GetByIdAsync(id);
        }

        public async Task<Tag> GetTagByName(string name)
        {
            return await _tagRepository.GetByNameAsync(name) ?? throw new EntityDoesNotExistException($"Tag with given name ({name}) does not exist");
        }
    }
}
