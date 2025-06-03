namespace AuctionHouseAPI.Application.DTOs.Read
{
    public record BidDTO(int UserId, int AuctionId, decimal Amount);
}
