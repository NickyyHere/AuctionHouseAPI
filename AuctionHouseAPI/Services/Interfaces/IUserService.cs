using AuctionHouseAPI.DTOs.Create;
using AuctionHouseAPI.DTOs.Read;
using AuctionHouseAPI.DTOs.Update;

namespace AuctionHouseAPI.Services.Interfaces
{
    public interface IUserService
    {
        public Task<int> CreateUser(CreateUserDTO createUserDTO);
        public Task UpdateUser(UpdateUserDTO updateUserDTO, int userId);
        public Task DeleteUser(int userId);
        public Task<UserDTO> GetUserById(int id);
        public Task<List<UserDTO>> GetAllUsers();
    }
}
