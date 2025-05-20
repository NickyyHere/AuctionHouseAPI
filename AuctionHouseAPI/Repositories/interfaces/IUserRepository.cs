using AuctionHouseAPI.Models;

namespace AuctionHouseAPI.Repositories.interfaces
{
    public interface IUserRepository
    {
        public Task<int> CreateUser(User user);
        public Task UpdateUser();
        public Task DeleteUser(User user);
        public Task<User> GetUserById(int id);
        public Task<User> GetUserByUsername(string username);
        public Task<List<User>> GetUsers(); 
    }
}
