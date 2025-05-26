using System.ComponentModel.DataAnnotations;

namespace AuctionHouseAPI.Application.DTOs.Read
{
    public class BidDTO
    {
        public int UserId { get; set; }
        public int AuctionId { get; set; }
        public decimal Amount { get; set; }

        public BidDTO(int userId, int auctionId, decimal amount)
        {
            UserId = userId;
            AuctionId = auctionId;
            Amount = amount;
        }
    }
}
