using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Application.Mappers
{
    public class AuctionMapper : IMapper<AuctionDTO, CreateAuctionDTO, Auction>
    {
        public AuctionDTO ToDTO(Auction entity)
        {
            var tags = entity.Item?.Tags?
                .Where(t => t.Tag != null)
                .Select(t => t.Tag!.Name)
                .ToList() ?? new List<string>();

            var auctionItemDTO = new AuctionItemDTO(
                entity.Id,
                entity.Item?.CategoryId ?? 0,
                entity.Item?.Name ?? string.Empty,
                entity.Item?.Description ?? string.Empty,
                tags);

            var auctionOptionsDTO = new AuctionOptionsDTO(
                entity.Id,
                entity.Options?.StartingPrice ?? 0,
                entity.Options?.StartDateTime ?? DateTime.MinValue,
                entity.Options?.FinishDateTime ?? DateTime.MinValue,
                entity.Options?.IsIncreamentalOnLastMinuteBid ?? false,
                entity.Options?.MinutesToIncrement ?? 0,
                entity.Options?.MinimumOutbid ?? 0,
                entity.Options?.AllowBuyItNow ?? false,
                entity.Options?.BuyItNowPrice ?? 0,
                entity.Options?.IsActive ?? false);

            return new AuctionDTO(
                entity.Id,
                entity.Owner?.FirstName ?? string.Empty,
                entity.Owner?.LastName ?? string.Empty,
                auctionItemDTO,
                auctionOptionsDTO);
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
            return auction;
        }
    }
}
