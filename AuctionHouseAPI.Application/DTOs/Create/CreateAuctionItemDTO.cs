namespace AuctionHouseAPI.Application.DTOs.Create
{
    public record CreateAuctionItemDTO(string Name, string Description, int CategoryId, List<string> CustomTags);
}
