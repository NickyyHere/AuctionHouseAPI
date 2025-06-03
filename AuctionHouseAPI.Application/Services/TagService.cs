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
            var tagId = await _tagRepository.CreateAsync(tag);
            return tagId;
        }

        public async Task<List<Tag>> EnsureTagsExistAsync(List<string> tagNames)
        {
            var result = new List<Tag>();
            await _tagRepository.BeginTransactionAsync();
            try
            {
                foreach (var name in tagNames.Distinct())
                {
                    var tag = await _tagRepository.GetByNameAsync(name);
                    if (tag == null)
                    {
                        var newTag = new Tag(name);
                        var newTagId = await _tagRepository.CreateAsync(newTag);
                        tag = await _tagRepository.GetByIdAsync(newTagId) ?? throw new EntityDoesNotExistException("Failed to fetch a newly created tag");
                    }
                    result.Add(tag);
                }
                await _tagRepository.CommitTransactionAsync();
                return result;
            }
            catch
            {
                await _tagRepository.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
