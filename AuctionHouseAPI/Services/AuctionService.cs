using AuctionHouseAPI.DTOs.Create;
using AuctionHouseAPI.DTOs.Read;
using AuctionHouseAPI.DTOs.Update;
using AuctionHouseAPI.Exceptions;
using AuctionHouseAPI.Mappers;
using AuctionHouseAPI.Models;
using AuctionHouseAPI.Repositories.interfaces;
using AuctionHouseAPI.Services.Interfaces;
using System.Linq;

namespace AuctionHouseAPI.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IMapper<AuctionDTO, CreateAuctionDTO, Auction> _mapper;
        public AuctionService(IAuctionRepository auctionRepository, ITagRepository tagRepository, IMapper<AuctionDTO, CreateAuctionDTO, Auction> mapper)
        {
            _auctionRepository = auctionRepository;
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateAuction(CreateAuctionDTO createAuctionDTO)
        {
            var auction = _mapper.ToEntity(createAuctionDTO);
            var id = await _auctionRepository.CreateAuction(auction);
            return id;
        }

        public async Task DeleteAuction(int id)
        {
            var auction = await _auctionRepository.GetAuctionById(id);
            await _auctionRepository.DeleteAuction(auction);
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

        public async Task UpdateAuctionItem(UpdateAuctionItemDTO updateAuctionItemDTO, int auctionId)
        {
            var auction = await _auctionRepository.GetAuctionById(auctionId);
            if(!string.IsNullOrWhiteSpace(updateAuctionItemDTO.Name)) 
                auction.Item.Name = updateAuctionItemDTO.Name;
            if(!string.IsNullOrWhiteSpace(updateAuctionItemDTO.Description))
                auction.Item.Description = updateAuctionItemDTO.Description;
            if(updateAuctionItemDTO.CustomTags.Count > 0)
            {
                var customTags = new List<Tag>();
                foreach (var tag in updateAuctionItemDTO.CustomTags)
                {
                    try
                    {
                        var tagObject = await _tagRepository.GetTagByName(tag);
                        customTags.Add(tagObject);
                    }
                    catch (EntityDoesNotExistException)
                    {
                        var newTag = await _tagRepository.CreateTag(new Tag(tag));
                        customTags.Add(newTag);
                    }
                }
                auction.Item.Tags = customTags.Select(t => new AuctionItemTag { TagId = t.Id, AuctionItemId = auction.Id }).ToList();
            }
            if (updateAuctionItemDTO.CategoryId != null)
                auction.Item.CategoryId = (int)updateAuctionItemDTO.CategoryId;
            await _auctionRepository.UpdateAuction();
        }

        public async Task UpdateAuctionOptions(UpdateAuctionOptionsDTO updateAuctionOptionsDTO, int auctionId)
        {
            var auction = await _auctionRepository.GetAuctionById(auctionId);
            if (updateAuctionOptionsDTO.IsIncreamentalOnLastMinuteBid != null)
                auction.Options.IsIncreamentalOnLastMinuteBid = (bool)updateAuctionOptionsDTO.IsIncreamentalOnLastMinuteBid;
            if (updateAuctionOptionsDTO.AllowBuyItNow != null)
                auction.Options.AllowBuyItNow = (bool)updateAuctionOptionsDTO.AllowBuyItNow;
            if (updateAuctionOptionsDTO.BuyItNowPrice != null)
                auction.Options.BuyItNowPrice = (decimal)updateAuctionOptionsDTO.BuyItNowPrice;
            if (updateAuctionOptionsDTO.StartingPrice != null)
                auction.Options.StartingPrice = (decimal)updateAuctionOptionsDTO.StartingPrice;
            if(updateAuctionOptionsDTO.FinishDateTime != null)
                auction.Options.FinishDateTime = (DateTime)updateAuctionOptionsDTO.FinishDateTime;
            if (updateAuctionOptionsDTO.MinimumOutbid != null)
                auction.Options.MinimumOutbid = (int)updateAuctionOptionsDTO.MinimumOutbid;
            if (updateAuctionOptionsDTO.MinutesToIncrement != null)
                auction.Options.MinutesToIncrement = (int)updateAuctionOptionsDTO.MinutesToIncrement;
            if (updateAuctionOptionsDTO.StartDateTime != null)
                auction.Options.StartDateTime = (DateTime)updateAuctionOptionsDTO.StartDateTime;
            await _auctionRepository.UpdateAuction();
        }
    }
}
