namespace AuctionHouseAPI.Application.DTOs.Read
{
    public record AuctionOptionsDTO(int AuctionId, decimal StartingPrice, DateTime StartDateTime, DateTime FinishDateTime, bool IsIncreamentalOnLastMinuteBid, int MinutesToIncrement, int MinimumOutbid, bool AllowBuyItNow, decimal BuyItNowPrice);
}
