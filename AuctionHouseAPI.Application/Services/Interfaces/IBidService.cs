using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Application.Services.Interfaces
{
    public interface IBidService
    {
        public Task CreateBidAsync(Bid bid, AuctionOptions auctionOptions, int userId);
        public Task WithdrawFromAuctionAsync(Auction auction, int userId);
    }
}
