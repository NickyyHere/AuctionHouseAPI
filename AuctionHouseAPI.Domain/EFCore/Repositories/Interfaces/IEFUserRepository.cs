using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Domain.EFCore.Repositories.Interfaces
{
    public interface IEFUserRepository : IEFCoreRepository<User>
    {
        public Task<User?> GetByUsernameAsync(string username);
    }
}
