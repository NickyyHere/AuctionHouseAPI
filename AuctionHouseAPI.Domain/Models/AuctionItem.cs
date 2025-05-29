using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionHouseAPI.Domain.Models
{
    public class AuctionItem
    {
        [Key, ForeignKey("Auction")]
        public int AuctionId { get; set; }
        public Auction? Auction { get; set; }
        // public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<AuctionItemTag> Tags { get; set; } = new List<AuctionItemTag>();
        public AuctionItem() 
        {
            Name = string.Empty;
            Description = string.Empty;
        }
        public AuctionItem(string name, string description, int categoryId) 
        {
            Name = name;
            Description = description;
            CategoryId = categoryId;
        }
    }
}
