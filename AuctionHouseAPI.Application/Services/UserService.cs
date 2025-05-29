using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
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
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateUser(CreateUserDTO createUserDTO)
        {
            if(await _userRepository.GetByUsernameAsync(createUserDTO.Username) != null)
                throw new DuplicateEntityException("Username is already taken");

            await _userRepository.BeginTransactionAsync();
            try
            {
                var user = _mapper.Map<User>(createUserDTO);
                var newId = await _userRepository.CreateAsync(user);
                await _userRepository.CommitTransactionAsync();
                return newId;
            }
            catch
            {
                await _userRepository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId) ?? throw new EntityDoesNotExistException($"User with given id ({userId}) does not exist");
            await _userRepository.BeginTransactionAsync();
            try
            {
                await _userRepository.DeleteAsync(user);
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
            var users = _mapper.Map<List<UserDTO>>(await _userRepository.GetAllAsync());
            return users;
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            var user = _mapper.Map<UserDTO>(await _userRepository.GetByIdAsync(id) ?? throw new EntityDoesNotExistException($"User with given id ({id}) does not exist"));
            return user;
        }

        public async Task UpdateUser(UpdateUserDTO updateUserDTO, int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId) ?? throw new EntityDoesNotExistException($"User with given id ({userId}) does not exist");
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
