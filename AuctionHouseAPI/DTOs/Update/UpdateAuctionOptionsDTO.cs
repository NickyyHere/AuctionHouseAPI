using AuctionHouseAPI.CustomValidators;
using System.ComponentModel.DataAnnotations;

namespace AuctionHouseAPI.DTOs.Update
{
    public class UpdateAuctionOptionsDTO
    {
        [Range(1, double.MaxValue)]
        public decimal? StartingPrice { get; set; }
        [ValidDateTime(AllowPast = false, ErrorMessage = "Start date can't be set in past")]
        public DateTime? StartDateTime { get; set; }
        [ValidDateTime(AllowPast = false, HoursOverlay = 1, ErrorMessage = "Finish date can't be set in past, and has to be at least 1 day after today")]
        public DateTime? FinishDateTime { get; set; }
        public bool? IsIncreamentalOnLastMinuteBid { get; set; } = false;
        public int? MinutesToIncrement { get; set; }
        [Range(5, double.MaxValue)]
        public int? MinimumOutbid { get; set; }
        public bool? AllowBuyItNow { get; set; } = false;
        public decimal? BuyItNowPrice { get; set; }
    }
}
