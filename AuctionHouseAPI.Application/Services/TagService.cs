using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace AuctionHouseAPI.Application.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly ILogger<TagService> _logger;

        public TagService(ITagRepository tagRepository, ILogger<TagService> logger)
        {
            _tagRepository = tagRepository;
            _logger = logger;
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
                        _logger.LogInformation("Tag {TagName} has been created", name);
                        tag = await _tagRepository.GetByIdAsync(newTagId) ?? throw new EntityDoesNotExistException("Failed to fetch a newly created tag");
                    }
                    result.Add(tag);
                }
                await _tagRepository.CommitTransactionAsync();
                return result;
            }
            catch (Exception ex)
            {
                if(ex is EntityDoesNotExistException)
                {
                    _logger.LogError(ex, "Error while fetching newly created tag");
                }
                await _tagRepository.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
