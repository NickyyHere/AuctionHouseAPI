﻿using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;

namespace AuctionHouseAPI.Application.Services.Interfaces
{
    public interface IBidService
    {
        public Task CreateBid(CreateBidDTO createBidDTO, int userId);
        public Task WithdrawFromAuction(int auctionId, int userId);
        public Task<List<BidDTO>> GetUserBids(int userId);
        public Task<List<BidDTO>> GetAuctionBids(int auctionId);
        public Task<List<BidDTO>> GetUsersBidsByAuctionId(int userId, int auctionId);
    }
}
