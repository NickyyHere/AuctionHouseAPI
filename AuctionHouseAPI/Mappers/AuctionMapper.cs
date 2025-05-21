using AuctionHouseAPI.DTOs.Create;
using AuctionHouseAPI.DTOs.Read;
using AuctionHouseAPI.Models;

namespace AuctionHouseAPI.Mappers
{
    public class AuctionMapper : IMapper<AuctionDTO, CreateAuctionDTO, Auction>
    {
        public AuctionDTO ToDTO(Auction entity)
        {
            #pragma warning disable CS8602 // disable possible null reference (i like clean console)
            var tags = entity.Item.Tags.Select(t => t.Tag.Name).ToList();
            var auctionItemDTO = new AuctionItemDTO(
                entity.Id, 
                entity.Item.
                CategoryId, 
                entity.Item.Name, 
                entity.Item.Description, 
                tags);
            var auctionOptionsDTO = new AuctionOptionsDTO(
                entity.Id, 
                entity.Options.StartingPrice, 
                entity.Options.StartDateTime, 
                entity.Options.FinishDateTime, 
                entity.Options.IsIncreamentalOnLastMinuteBid, 
                entity.Options.MinutesToIncrement,
                entity.Options.MinimumOutbid,
                entity.Options.AllowBuyItNow, 
                entity.Options.BuyItNowPrice,
                entity.Options.IsActive);

            return new AuctionDTO(entity.Id, entity.Owner.FirstName, entity.Owner.LastName, auctionItemDTO, auctionOptionsDTO);
        }

        public List<AuctionDTO> ToDTO(List<Auction> entities)
        {
            var DTOs = new List<AuctionDTO>();
            foreach (var entity in entities)
            {
                DTOs.Add(ToDTO(entity));
            }
            return DTOs;
        }

        public Auction ToEntity(CreateAuctionDTO create_dto)
        {
            var auctionItem = new AuctionItem(create_dto.Item.Name, create_dto.Item.Description, create_dto.Item.CategoryId);
            var auctionOptions = new AuctionOptions(
                create_dto.Options.StartingPrice,
                create_dto.Options.StartDateTime ?? DateTime.Now,
                create_dto.Options.FinishDateTime,
                create_dto.Options.IsIncreamentalOnLastMinuteBid,
                create_dto.Options.MinutesToIncrement ?? 0,
                create_dto.Options.MinimumOutbid,
                create_dto.Options.AllowBuyItNow,
                create_dto.Options.BuyItNowPrice ?? 0,
                create_dto.Options.StartDateTime > DateTime.Now ? false : true
                );
            var auction = new Auction(auctionItem, auctionOptions);
            auction.Item.Tags = create_dto.Item.CustomTags.Select(t => new AuctionItemTag { Tag = new Tag(t)}).ToList();
            return auction;
        }
    }
}
