using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Domain.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>, ITransactionRepository
    {
        public Task UpdateCategoryAsync(Category category);
    }
}
