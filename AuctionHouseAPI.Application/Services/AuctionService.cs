using AuctionHouseAPI.Application.DTOs.Update;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Shared.Exceptions;

namespace AuctionHouseAPI.Application.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;
        public AuctionService(IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
        }

        public async Task<int> CreateAuctionAsync(Auction auction)
        {
            var newId = await _auctionRepository.CreateAsync(auction);
            return newId;
        }
        public void AddTagsToAuction(List<Tag> tags, Auction auction)
        {
            foreach (var tag in tags)
            {
                auction.Item!.Tags.Add(new AuctionItemTag { TagId = tag.Id });
            }
        }
        public async Task DeleteAuctionAsync(Auction auction, int userId)
        {
            await _auctionRepository.BeginTransactionAsync();
            try
            {
                if (auction.Options!.StartDateTime <= DateTime.UtcNow && auction.Options.FinishDateTime >= DateTime.UtcNow)
                {
                    throw new ActiveAuctionException("Can't delete active auction");
                }
                if (auction.OwnerId != userId)
                {
                    throw new UnauthorizedAccessException("Access denied! Only auction owner can delete the auction.");
                }
                if (auction.Options.FinishDateTime < DateTime.UtcNow)
                {
                    throw new FinishedAuctionException("Can't delete finished auction");
                }
                await _auctionRepository.DeleteAsync(auction);
                await _auctionRepository.CommitTransactionAsync();
            }
            catch
            {
                await _auctionRepository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateAuctionItemAsync(Auction auction, UpdateAuctionItemDTO updateAuctionItemDTO, int userId)
        {
            await _auctionRepository.BeginTransactionAsync();
            try
            {
                if (auction.OwnerId != userId)
                {
                    throw new UnauthorizedAccessException("Access denied! Only auction owner can edit auction item.");
                }
                if (auction.Options!.StartDateTime <= DateTime.UtcNow && auction.Options.FinishDateTime >= DateTime.UtcNow)
                {
                    throw new ActiveAuctionException("Can't edit active auction");
                }
                if(auction.Options.FinishDateTime < DateTime.UtcNow)
                {
                    throw new FinishedAuctionException("Can't edit finished auction");
                }
                if (!string.IsNullOrWhiteSpace(updateAuctionItemDTO.Name))
                {
                    auction.Item!.Name = updateAuctionItemDTO.Name;
                }
                if (!string.IsNullOrWhiteSpace(updateAuctionItemDTO.Description))
                {
                    auction.Item!.Description = updateAuctionItemDTO.Description;
                }
                if (updateAuctionItemDTO.CategoryId != null)
                {
                    auction.Item!.CategoryId = (int)updateAuctionItemDTO.CategoryId;
                }

                await _auctionRepository.UpdateAuctionItemAsync(auction.Item!);
                await _auctionRepository.CommitTransactionAsync();
            }
            catch
            {
                await _auctionRepository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateAuctionOptionsAsync(Auction auction, UpdateAuctionOptionsDTO updateAuctionOptionsDTO, int userId)
        {
            await _auctionRepository.BeginTransactionAsync();
            try
            {
                if (auction.OwnerId != userId)
                {
                    throw new UnauthorizedAccessException("Access denied! Only auction owner can edit auction options.");
                }
                if (auction.Options!.StartDateTime <= DateTime.UtcNow && auction.Options.FinishDateTime >= DateTime.UtcNow)
                {
                    throw new ActiveAuctionException("Can't edit active auction");
                }
                if (auction.Options.FinishDateTime < DateTime.UtcNow)
                {
                    throw new FinishedAuctionException("Can't edit finished auction");
                }
                if (updateAuctionOptionsDTO.IsIncreamentalOnLastMinuteBid != null)
                {
                    auction.Options!.IsIncreamentalOnLastMinuteBid = (bool)updateAuctionOptionsDTO.IsIncreamentalOnLastMinuteBid;
                }

                if (updateAuctionOptionsDTO.AllowBuyItNow != null)
                {
                    auction.Options!.AllowBuyItNow = (bool)updateAuctionOptionsDTO.AllowBuyItNow;
                }

                if (updateAuctionOptionsDTO.BuyItNowPrice != null)
                {
                    auction.Options!.BuyItNowPrice = (decimal)updateAuctionOptionsDTO.BuyItNowPrice;
                }

                if (updateAuctionOptionsDTO.StartingPrice != null)
                {
                    auction.Options!.StartingPrice = (decimal)updateAuctionOptionsDTO.StartingPrice;
                }

                if (updateAuctionOptionsDTO.FinishDateTime != null)
                {
                    auction.Options!.FinishDateTime = (DateTime)updateAuctionOptionsDTO.FinishDateTime;
                }

                if (updateAuctionOptionsDTO.MinimumOutbid != null)
                {
                    auction.Options!.MinimumOutbid = (int)updateAuctionOptionsDTO.MinimumOutbid;
                }

                if (updateAuctionOptionsDTO.MinutesToIncrement != null)
                {
                    auction.Options!.MinutesToIncrement = (int)updateAuctionOptionsDTO.MinutesToIncrement;
                }

                if (updateAuctionOptionsDTO.StartDateTime != null)
                {
                    auction.Options!.StartDateTime = (DateTime)updateAuctionOptionsDTO.StartDateTime;
                }
                await _auctionRepository.UpdateAuctionOptionsAsync(auction.Options!);
                await _auctionRepository.CommitTransactionAsync();
            }
            catch
            {
                await _auctionRepository.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
