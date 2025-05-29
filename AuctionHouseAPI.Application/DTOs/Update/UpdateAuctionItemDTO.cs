namespace AuctionHouseAPI.Application.DTOs.Update
{
    public record UpdateAuctionItemDTO(string? Name, string? Description, int? CategoryId, List<string> CustomTags);
}
