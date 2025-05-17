using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AuctionHouseAPI.Models
{
    public class AuctionItem
    {
        [Key, ForeignKey("AuctionId")]
        public int AuctionId { get; set; }
        [JsonIgnore]
        public Auction? Auction { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        [JsonIgnore]
        public Category? Category { get; set; }
        public List<string> CustomTags { get; set; }
    }
}
