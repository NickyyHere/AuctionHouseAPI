using System.ComponentModel.DataAnnotations;

namespace AuctionHouseAPI.DTOs.Create
{
#pragma warning disable
    public class CreateAuctionItemDTO
    {
        [Required, MaxLength(255), MinLength(3)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public List<string> CustomTags { get; set; }
    }
}
