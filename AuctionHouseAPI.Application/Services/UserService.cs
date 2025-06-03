using AuctionHouseAPI.Application.DTOs.Update;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Shared.Exceptions;
using AutoMapper;

namespace AuctionHouseAPI.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> CreateUserAsync(User user)
        {
            if(await _userRepository.GetByUsernameAsync(user.Username) != null)
                throw new DuplicateEntityException("Username is already taken");

            var newId = await _userRepository.CreateAsync(user);
            return newId;
        }

        public async Task DeleteUserAsync(User user)
        {

            await _userRepository.DeleteAsync(user);
        }
        public async Task UpdateUserAsync(User user, UpdateUserDTO updateUserDTO)
        {
            await _userRepository.BeginTransactionAsync();
            try
            {
                if (!string.IsNullOrWhiteSpace(updateUserDTO.FirstName))
                    user.FirstName = updateUserDTO.FirstName;
                if (!string.IsNullOrWhiteSpace(updateUserDTO.Email))
                    user.Email = updateUserDTO.Email;
                if (!string.IsNullOrWhiteSpace(updateUserDTO.LastName))
                    user.LastName = updateUserDTO.LastName;
                if (!string.IsNullOrWhiteSpace(updateUserDTO.Password))
                    user.Password = BCrypt.Net.BCrypt.HashPassword(updateUserDTO.Password);
                await _userRepository.UpdateUserAsync(user);
                await _userRepository.CommitTransactionAsync();
            }
            catch
            {
                await _userRepository.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
