namespace AuctionHouseAPI.Application.DTOs.Read
{
    public record AuctionDTO(int Id, string OwnerFirstName, string OwnerLastName, AuctionItemDTO Item, AuctionOptionsDTO Options);
}
