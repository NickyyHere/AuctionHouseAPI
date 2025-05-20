using AuctionHouseAPI.DTOs.Create;
using AuctionHouseAPI.DTOs.Read;
using AuctionHouseAPI.Models;
using BCrypt.Net;


namespace AuctionHouseAPI.Mappers
{
    public class UserMapper : IMapper<UserDTO, CreateUserDTO, User>
    {
        public UserDTO ToDTO(User entity)
        {
            return new UserDTO(entity.Id, entity.Email, entity.FirstName, entity.LastName, entity.JoinedDateTime);
        }

        public List<UserDTO> ToDTO(List<User> entities)
        {
            var DTOs = new List<UserDTO>();
            foreach (var entity in entities)
            {
                DTOs.Add(ToDTO(entity));
            }
            return DTOs;
        }

        public User ToEntity(CreateUserDTO create_dto)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(create_dto.Password);
            return new User(create_dto.Username, create_dto.Email, hashedPassword, create_dto.FirstName, create_dto.LastName);
        }
    }
}
