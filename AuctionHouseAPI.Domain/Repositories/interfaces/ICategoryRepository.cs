using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Domain.Repositories.interfaces
{
    public interface ICategoryRepository : IBaseRepository
    {
        public Task CreateCategory(Category category);
        public void DeleteCategory(Category category);
        public Task<Category> GetCategoryById(int id);
        public Task<List<Category>> GetCategories();
    }
}
