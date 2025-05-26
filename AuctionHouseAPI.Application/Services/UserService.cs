using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Application.DTOs.Update;
using AuctionHouseAPI.Application.Mappers;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Domain.Repositories.interfaces;
using AuctionHouseAPI.Shared.Exceptions;

namespace AuctionHouseAPI.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper<UserDTO, CreateUserDTO, User> _mapper;
        public UserService(IUserRepository userRepository, IMapper<UserDTO, CreateUserDTO, User> mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateUser(CreateUserDTO createUserDTO)
        {
            try
            {
                await _userRepository.GetUserByUsername(createUserDTO.Username);
            }
            catch (EntityDoesNotExistException)
            {
                await _userRepository.BeginTransactionAsync();
                try
                {
                    var user = _mapper.ToEntity(createUserDTO);
                    await _userRepository.CreateUser(user);
                    await _userRepository.CommitTransactionAsync();
                    return user.Id;
                }
                catch
                {
                    await _userRepository.RollbackTransactionAsync();
                    throw;
                }
            }
            throw new DuplicateEntityException("Username is already taken");
        }

        public async Task DeleteUser(int userId)
        {
            await _userRepository.BeginTransactionAsync();
            try
            {
                var user = await _userRepository.GetUserById(userId);
                _userRepository.DeleteUser(user);
                await _userRepository.CommitTransactionAsync();
            }
            catch
            {
                await _userRepository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<List<UserDTO>> GetAllUsers()
        {
            var users = _mapper.ToDTO(await _userRepository.GetUsers());
            return users;
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            var user = _mapper.ToDTO(await _userRepository.GetUserById(id));
            return user;
        }

        public async Task UpdateUser(UpdateUserDTO updateUserDTO, int userId)
        {
            await _userRepository.BeginTransactionAsync();
            try
            {
                var user = await _userRepository.GetUserById(userId);
                if (!string.IsNullOrWhiteSpace(updateUserDTO.FirstName))
                    user.FirstName = updateUserDTO.FirstName;
                if (!string.IsNullOrWhiteSpace(updateUserDTO.Email))
                    user.Email = updateUserDTO.Email;
                if (!string.IsNullOrWhiteSpace(updateUserDTO.LastName))
                    user.LastName = updateUserDTO.LastName;
                if (!string.IsNullOrWhiteSpace(updateUserDTO.Password))
                    user.Password = BCrypt.Net.BCrypt.HashPassword(updateUserDTO.Password);
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
