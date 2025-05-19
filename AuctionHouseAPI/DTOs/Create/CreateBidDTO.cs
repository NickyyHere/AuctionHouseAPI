using System.ComponentModel.DataAnnotations;

namespace AuctionHouseAPI.DTOs.Create
{
    public class CreateBidDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int AuctionId { get; set; }
        [Required, Range(1, int.MaxValue)]
        public decimal Amount { get; set; }
    }
}
