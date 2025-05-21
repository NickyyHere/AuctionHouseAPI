using AuctionHouseAPI.DTOs.Create;
using AuctionHouseAPI.DTOs.Read;
using AuctionHouseAPI.DTOs.Update;
using AuctionHouseAPI.Exceptions;
using AuctionHouseAPI.Mappers;
using AuctionHouseAPI.Models;
using AuctionHouseAPI.Repositories.interfaces;
using AuctionHouseAPI.Services.Interfaces;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace AuctionHouseAPI.Services
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
            if(await _userRepository.GetUserByUsername(createUserDTO.Username) != null)
            {
                throw new DuplicateEntityException($"Username is already in use");
            }
            var user = _mapper.ToEntity(createUserDTO);
            var id = await _userRepository.CreateUser(user);
            return id;
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            await _userRepository.DeleteUser(user);
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
            var user = await _userRepository.GetUserById(userId);
            if(!string.IsNullOrWhiteSpace(updateUserDTO.FirstName))
                user.FirstName = updateUserDTO.FirstName;
            if(!string.IsNullOrWhiteSpace(updateUserDTO.Email))
                user.Email = updateUserDTO.Email;
            if(!string.IsNullOrWhiteSpace(updateUserDTO.LastName))
                user.LastName = updateUserDTO.LastName;
            if(!string.IsNullOrWhiteSpace(updateUserDTO.Password))
                user.Password = updateUserDTO.Password;
            await _userRepository.UpdateUser();
        }
    }
}
