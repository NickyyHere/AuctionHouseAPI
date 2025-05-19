using System.ComponentModel.DataAnnotations;

namespace AuctionHouseAPI.DTOs.Create
{
#pragma warning disable
    public class CreateAuctionDTO
    {
        [Required]
        public int OwnerId { get; set; }
        [Required]
        public CreateAuctionItemDTO Item { get; set; }
        [Required]
        public CreateAuctionOptionsDTO Options { get; set; }
    }
}
