using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Application.DTOs.Update;
using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Application.Services.Interfaces
{
    public interface IUserService
    {
        public Task<int> CreateUserAsync(User user);
        public Task UpdateUserAsync(User user, UpdateUserDTO updateUserDTO);
        public Task DeleteUserAsync(User user);
    }
}
