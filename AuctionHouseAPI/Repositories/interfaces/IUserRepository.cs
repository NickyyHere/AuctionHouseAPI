using AuctionHouseAPI.Models;

namespace AuctionHouseAPI.Repositories.interfaces
{
    public interface IUserRepository
    {
        public Task CreateUser(User user);
        public Task UpdateUser();
        public Task DeleteUser(User user);
        public Task<User> GetUserById(int id);
        public Task<List<User>> GetUsers(); 
    }
}
