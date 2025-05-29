namespace AuctionHouseAPI.Application.DTOs.Create
{
    public record CreateAuctionOptionsDTO(decimal StartingPrice, DateTime? StartDateTime, DateTime FinishDateTime, bool IsIncreamentalOnLastMinuteBid, int MinutesToIncrement, int MinimumOutbid, bool AllowBuyItNow, decimal BuyItNowPrice);
}
