using AuctionHouseAPI.Application.DTOs.Update;
using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Application.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<int> CreateCategoryAsync(Category category);
        public Task UpdateCategoryAsync(Category category, UpdateCategoryDTO categoryDTO);
        public Task DeleteCategoryAsync(Category category);
    }
}
