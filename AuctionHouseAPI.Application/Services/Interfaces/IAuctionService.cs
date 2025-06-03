using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Application.DTOs.Update;
using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Application.Services.Interfaces
{
    public interface IAuctionService
    {
        public Task<int> CreateAuctionAsync(Auction auction, int userId);
        public void AddTagsToAuction(List<Tag> tags, Auction auction);
        public Task UpdateAuctionItemAsync(Auction auction, UpdateAuctionItemDTO updateAuctionItemDTO, int userId);
        public Task UpdateAuctionOptionsAsync(Auction auction, UpdateAuctionOptionsDTO updateAuctionOptionsDTO, int userId);
        public Task DeleteAuction(Auction auction, int userId);
    }
}
