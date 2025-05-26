using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Domain.Repositories.interfaces
{
    public interface IUserRepository : IBaseRepository
    {
        public Task CreateUser(User user);
        public void DeleteUser(User user);
        public Task<User> GetUserById(int id);
        public Task<User> GetUserByUsername(string username);
        public Task<List<User>> GetUsers(); 
    }
}
