using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Application.DTOs.Update;

namespace AuctionHouseAPI.Application.Services.Interfaces
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
