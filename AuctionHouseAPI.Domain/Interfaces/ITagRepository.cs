using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Domain.Interfaces
{
    public interface ITagRepository : IBaseRepository<Tag>, ITransactionRepository
    {
        public Task<Tag?> GetByNameAsync(string name);
    }
}
