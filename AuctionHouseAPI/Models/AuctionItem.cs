using AuctionHouseAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionHouseAPI.Models
{
    public class AuctionItem
    {
        [Key, ForeignKey("AuctionId")]
        public int AuctionId { get; set; }
        public Auction Auction { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public List<string> CustomTags { get; set; }
    }
}
