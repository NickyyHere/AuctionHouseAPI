using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AuctionHouseAPI.Models
{
    public class AuctionOptions
    {
        [Key, ForeignKey("AuctionId")]
        public int AuctionId { get; set; }
        [JsonIgnore]
        public Auction? Auction { get; set; }
        public decimal StartingPrice { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime FinishDateTime { get; set; }
        public bool IsIncreamentalOnLastMinuteBid { get; set; }
        public int MinutesToIncrement { get; set; }
        public int MinimumOutbid { get; set; }
        public bool AllowBuyItNow { get; set; }
        public decimal BuyItNowPrice { get; set; }
        public bool IsActive { get; set; }
    }
}
