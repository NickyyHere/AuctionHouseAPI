using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Domain.EFCore.Repositories.Interfaces
{
    public interface IEFTagRepository : IEFCoreRepository<Tag>
    {
        public Task<Tag?> GetByNameAsync(string name);
    }
}
