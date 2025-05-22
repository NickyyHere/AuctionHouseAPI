using System.ComponentModel.DataAnnotations;

namespace AuctionHouseAPI.Application.DTOs.Create
{
    public class CreateBidDTO
    {
        [Required]
        public int AuctionId { get; set; }
        [Required, Range(1, int.MaxValue)]
        public decimal Amount { get; set; }
    }
}
