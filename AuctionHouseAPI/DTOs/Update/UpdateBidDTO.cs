using System.ComponentModel.DataAnnotations;

namespace AuctionHouseAPI.DTOs.Update
{
    public class UpdateBidDTO
    {
        [Range(1, int.MaxValue)]
        public decimal Amount { get; set; }
    }
}
