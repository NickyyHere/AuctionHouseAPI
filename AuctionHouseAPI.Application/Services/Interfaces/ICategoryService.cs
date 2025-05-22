using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Application.DTOs.Update;

namespace AuctionHouseAPI.Application.Services.Interfaces
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
