using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Domain.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>, ITransactionRepository
    {
        public Task<User?> GetByUsernameAsync(string username);
        public Task UpdateUserAsync(User user);
    }
}
