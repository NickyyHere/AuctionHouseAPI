using System.Text.Json.Serialization;

namespace AuctionHouseAPI.Models
{
    public class Bid
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int AuctionId { get; set; }
        public Auction? Auction { get; set; }
        public decimal Amount { get; set; }
        public DateTime PlacedDateTime { get; set; }
        public Bid() { }
        public Bid(int auctionId, decimal amount)
        {
            AuctionId = auctionId;
            Amount = amount;
            PlacedDateTime = DateTime.Now;
        }
    }
}
