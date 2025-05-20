using AuctionHouseAPI.DTOs.Create;
using AuctionHouseAPI.DTOs.Read;
using AuctionHouseAPI.DTOs.Update;

namespace AuctionHouseAPI.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task CreateCategory(CreateCategoryDTO categoryDTO);
        public Task UpdateCategory(UpdateCategoryDTO categoryDTO, int id);
        public Task DeleteCategory(int id);
        public Task<CategoryDTO> GetCategory(int id);
        public Task<List<CategoryDTO>> GetAllCategories();
    }
}
