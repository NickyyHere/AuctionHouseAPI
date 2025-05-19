using System.Diagnostics.Contracts;

namespace AuctionHouseAPI.Models
{
    public class AuctionItemTag
    {
        public int AuctionItemId { get; set; }
        public AuctionItem? AuctionItem { get; set; }
        public int TagId { get; set; }
        public Tag? Tag { get; set; }
    }
}
