namespace AuctionHouseAPI.Application.DTOs.Update
{
    public record UpdateAuctionOptionsDTO(decimal? StartingPrice, DateTime? StartDateTime, DateTime? FinishDateTime, bool? IsIncreamentalOnLastMinuteBid, int? MinutesToIncrement, int? MinimumOutbid, bool? AllowBuyItNow, decimal? BuyItNowPrice);
}
