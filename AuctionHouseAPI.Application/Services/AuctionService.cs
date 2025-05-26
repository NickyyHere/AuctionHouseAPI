using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Application.DTOs.Update;
using AuctionHouseAPI.Application.Mappers;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Domain.Repositories.interfaces;
using AuctionHouseAPI.Shared.Exceptions;

namespace AuctionHouseAPI.Application.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly ITagService _tagService;
        private readonly IMapper<AuctionDTO, CreateAuctionDTO, Auction> _mapper;
        public AuctionService(IAuctionRepository auctionRepository, ITagService tagService, IMapper<AuctionDTO, CreateAuctionDTO, Auction> mapper)
        {
            _auctionRepository = auctionRepository;
            _tagService = tagService;
            _mapper = mapper;
        }

        public async Task<int> CreateAuction(CreateAuctionDTO createAuctionDTO, int userId)
        {
            await _auctionRepository.BeginTransactionAsync();
            try
            {
                var auction = _mapper.ToEntity(createAuctionDTO);
                auction.OwnerId = userId;
                await _auctionRepository.CreateAuction(auction);
                await _auctionRepository.SaveChangesAsync();
                await AddTagsToAuctionItem(createAuctionDTO.Item.CustomTags, auction.Item!);
                await _auctionRepository.CommitTransactionAsync();
                return auction.Id;
            }
            catch
            {
                await _auctionRepository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task DeleteAuction(int id, int userId)
        {
            await _auctionRepository.BeginTransactionAsync();
            try
            {
                var auction = await _auctionRepository.GetAuctionById(id);
                if (auction.Options!.IsActive)
                {
                    throw new ActiveAuctionException("Can't delete active auction");
                }
                if (auction.OwnerId != userId)
                {
                    throw new UnauthorizedAccessException("Access denied! Only auction owner can delete the auction.");
                }
                _auctionRepository.DeleteAuction(auction);
                await _auctionRepository.CommitTransactionAsync();
            }
            catch
            {
                await _auctionRepository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<List<AuctionItemDTO>> GetAllAuctionItems()
        {
            var auctions = _mapper.ToDTO(await _auctionRepository.GetAuctions());
            var items = auctions.Select(t => t.Item).ToList();
            return items;
        }

        public async Task<List<AuctionDTO>> GetAllAuctions()
        {
            var auctions = _mapper.ToDTO(await _auctionRepository.GetAuctions());
            return auctions;
        }

        public async Task<AuctionDTO> GetAuctionById(int id)
        {
            var auction = _mapper.ToDTO(await _auctionRepository.GetAuctionById(id));
            return auction;
        }

        public async Task<AuctionItemDTO> GetAuctionItem(int auctionId)
        {
            var auction = _mapper.ToDTO(await _auctionRepository.GetAuctionById(auctionId));
            return auction.Item;
        }

        public async Task<AuctionOptionsDTO> GetAuctionOptions(int auctionId)
        {
            var auction = _mapper.ToDTO(await _auctionRepository.GetAuctionById(auctionId));
            return auction.Options;
        }

        public async Task<List<AuctionDTO>> GetAuctionsByCategory(int categoryId)
        {
            var auctions = _mapper.ToDTO(await _auctionRepository.GetAuctionsByCategoryId(categoryId));
            return auctions;
        }

        public async Task<List<AuctionDTO>> GetAuctionsByTags(string[] tags)
        {
            var tasks = tags.Select(t => _auctionRepository.GetAuctionsByTag(t)).ToList();
            var results = await Task.WhenAll(tasks);

            var auctions = new HashSet<AuctionDTO>();
            foreach (var task in results)
            {
                foreach (var auction in task)
                {
                    auctions.Add(_mapper.ToDTO(auction));
                }
            }
            return auctions.ToList();
        }

        public async Task<List<AuctionDTO>> GetAuctionsByUser(int userId)
        {
            var auctions = _mapper.ToDTO(await _auctionRepository.GetUserAuctions(userId));
            return auctions;
        }

        public async Task UpdateAuctionItem(UpdateAuctionItemDTO updateAuctionItemDTO, int auctionId, int userId)
        {
            await _auctionRepository.BeginTransactionAsync();
            try
            {
                var auction = await _auctionRepository.GetAuctionById(auctionId);
                if (auction.OwnerId != userId)
                {
                    throw new UnauthorizedAccessException("Access denied! Only auction owner can edit auction item.");
                }
                if (auction.Options!.IsActive)
                {
                    throw new ActiveAuctionException("Can't edit active auction");
                }
                if (!string.IsNullOrWhiteSpace(updateAuctionItemDTO.Name))
                    auction.Item!.Name = updateAuctionItemDTO.Name;
                if (!string.IsNullOrWhiteSpace(updateAuctionItemDTO.Description))
                    auction.Item!.Description = updateAuctionItemDTO.Description;
                if (updateAuctionItemDTO.CategoryId != null)
                    auction.Item!.CategoryId = (int)updateAuctionItemDTO.CategoryId;
                await AddTagsToAuctionItem(updateAuctionItemDTO.CustomTags, auction.Item!);
                await _auctionRepository.CommitTransactionAsync();
            }
            catch
            {
                await _auctionRepository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateAuctionOptions(UpdateAuctionOptionsDTO updateAuctionOptionsDTO, int auctionId, int userId)
        {
            await _auctionRepository.BeginTransactionAsync();
            try
            {
                var auction = await _auctionRepository.GetAuctionById(auctionId);
                if (auction.OwnerId != userId)
                {
                    throw new UnauthorizedAccessException("Access denied! Only auction owner can edit auction options.");
                }
                if (auction.Options!.IsActive)
                {
                    throw new ActiveAuctionException("Can't edit active auction");
                }
                if (updateAuctionOptionsDTO.IsIncreamentalOnLastMinuteBid != null)
                    auction.Options!.IsIncreamentalOnLastMinuteBid = (bool)updateAuctionOptionsDTO.IsIncreamentalOnLastMinuteBid;
                if (updateAuctionOptionsDTO.AllowBuyItNow != null)
                    auction.Options!.AllowBuyItNow = (bool)updateAuctionOptionsDTO.AllowBuyItNow;
                if (updateAuctionOptionsDTO.BuyItNowPrice != null)
                    auction.Options!.BuyItNowPrice = (decimal)updateAuctionOptionsDTO.BuyItNowPrice;
                if (updateAuctionOptionsDTO.StartingPrice != null)
                    auction.Options!.StartingPrice = (decimal)updateAuctionOptionsDTO.StartingPrice;
                if (updateAuctionOptionsDTO.FinishDateTime != null)
                    auction.Options!.FinishDateTime = (DateTime)updateAuctionOptionsDTO.FinishDateTime;
                if (updateAuctionOptionsDTO.MinimumOutbid != null)
                    auction.Options!.MinimumOutbid = (int)updateAuctionOptionsDTO.MinimumOutbid;
                if (updateAuctionOptionsDTO.MinutesToIncrement != null)
                    auction.Options!.MinutesToIncrement = (int)updateAuctionOptionsDTO.MinutesToIncrement;
                if (updateAuctionOptionsDTO.StartDateTime != null)
                    auction.Options!.StartDateTime = (DateTime)updateAuctionOptionsDTO.StartDateTime;
                await _auctionRepository.CommitTransactionAsync();
            }
            catch
            {
                await _auctionRepository.RollbackTransactionAsync();
                throw;
            }
        }

        private async Task AddTagsToAuctionItem(List<string> tags, AuctionItem auctionItem)
        {
            if (tags.Count > 0)
            {
                var customTags = new List<Tag>();
                foreach (var tag in tags)
                {
                    try
                    {
                        var tagObject = await _tagService.GetTagByName(tag);
                        customTags.Add(tagObject);
                    }
                    catch (EntityDoesNotExistException)
                    {
                        var newTag = new Tag(tag);
                        await _tagService.CreateTag(newTag);
                        await _auctionRepository.SaveChangesAsync();
                        customTags.Add(newTag);
                    }
                }
                auctionItem.Tags = customTags.Select(t => new AuctionItemTag { TagId = t.Id, AuctionItemId = auctionItem.AuctionId }).ToList();
            }
        }
    }
}
