using System.Text.Json.Serialization;

namespace AuctionHouseAPI.Models
{
    public class Auction
    {
        public int Id { get; set; }
        public AuctionItem? Item { get; set; }
        public AuctionOptions? Options { get; set; }
        public int OwnerId { get; set; }
        public User? Owner { get; set; }
        public ICollection<Bid> Bids { get; set; } = new List<Bid>();
        public Auction() {}
        public Auction(AuctionItem item, AuctionOptions options)
        {
            Item = item;
            Options = options;
        }
    }
}
