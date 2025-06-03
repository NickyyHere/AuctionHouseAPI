namespace AuctionHouseAPI.Application.DTOs.Read
{
    public record AuctionItemDTO(int AuctionId, int CategoryId, string Name, string Description, List<string> Tags);
}
