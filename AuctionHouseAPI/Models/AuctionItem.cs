using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionHouseAPI.Models
{
    public class AuctionItem
    {
        [Key, ForeignKey("Auction")]
        public int AuctionId { get; set; }
        public Auction? Auction { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<AuctionItemTag> Tags { get; set; }

        public AuctionItem(int auctionId, string name, string description, int categoryId) 
        {
            AuctionId = auctionId;
            Name = name;
            Description = description;
            CategoryId = categoryId;
            Tags = new List<AuctionItemTag>();
        }
    }
}
