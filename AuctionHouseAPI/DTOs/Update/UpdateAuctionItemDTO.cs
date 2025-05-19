using AuctionHouseAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuctionHouseAPI.DTOs.Update
{
    public class UpdateAuctionItemDTO
    {
        [MaxLength(255), MinLength(3)]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public List<string> CustomTags { get; set; } = new();
    }
}
