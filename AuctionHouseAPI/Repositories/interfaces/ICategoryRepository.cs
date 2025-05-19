using AuctionHouseAPI.Models;

namespace AuctionHouseAPI.Repositories.interfaces
{
    public interface ICategoryRepository
    {
        public Task CreateCategory(Category category);
        public Task UpdateCategory();
        public Task DeleteCategory(Category category);
        public Task<Category> GetCategoryById(int id);
        public Task<List<Category>> GetCategories();
    }
}
