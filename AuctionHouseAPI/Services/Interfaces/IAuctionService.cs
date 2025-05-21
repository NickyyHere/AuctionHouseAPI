using AuctionHouseAPI.DTOs.Create;
using AuctionHouseAPI.DTOs.Read;
using AuctionHouseAPI.DTOs.Update;

namespace AuctionHouseAPI.Services.Interfaces
{
    public interface IAuctionService
    {
        public Task<int> CreateAuction(CreateAuctionDTO createAuctionDTO, int userId);
        public Task UpdateAuctionItem(UpdateAuctionItemDTO updateAuctionItemDTO, int auctionId, int userId);
        public Task UpdateAuctionOptions(UpdateAuctionOptionsDTO updateAuctionOptionsDTO, int auctionId, int userId);
        public Task DeleteAuction(int id, int userId);
        public Task<AuctionDTO> GetAuctionById(int id);
        public Task<List<AuctionDTO>> GetAllAuctions();
        public Task<List<AuctionDTO>> GetAuctionsByUser(int userId);
        public Task<List<AuctionDTO>> GetAuctionsByCategory(int categoryId);
        public Task<List<AuctionDTO>> GetAuctionsByTags(string[] tags);
        public Task<List<AuctionItemDTO>> GetAllAuctionItems();
        public Task<AuctionOptionsDTO> GetAuctionOptions(int auctionId);
        public Task<AuctionItemDTO> GetAuctionItem(int auctionId);
    }
}
