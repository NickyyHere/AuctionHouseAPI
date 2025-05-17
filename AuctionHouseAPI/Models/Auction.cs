namespace AuctionHouseAPI.Models
{
    public class Auction
    {
        public int Id { get; set; }
        public AuctionItem Item { get; set; }
        public AuctionOptions Options { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
    }
}
