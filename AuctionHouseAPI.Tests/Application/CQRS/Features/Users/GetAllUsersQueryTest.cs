using AuctionHouseAPI.Application.CQRS.Features.Users.Handlers;
using AuctionHouseAPI.Application.CQRS.Features.Users.Queries;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AutoMapper;
using Moq;

namespace AuctionHouseAPI.Tests.Application.CQRS.Features.Users
{
    public class GetAllUsersQueryTest
    {
        [Test]
        public async Task ShouldCallRepositoryAndMapToDTOs()
        {
            var repository = new Mock<IUserRepository>();
            var mapper = new Mock<IMapper>();

            var users = new List<User>
            {
                new User(),
                new User()
            };
            var usersDtos = new List<UserDTO>
            {
                new UserDTO(1, "", "", "", DateTime.Now, Domain.Enums.UserRole.ROLE_USER),
                new UserDTO(2, "", "", "", DateTime.Now, Domain.Enums.UserRole.ROLE_USER)
            };
            repository.Setup(r => r.GetAllAsync()).ReturnsAsync(users);
            mapper.Setup(m => m.Map<List<UserDTO>>(users)).Returns(usersDtos);

            var query = new GetAllUsersQuery();
            var handler = new GetAllUsersHandler(repository.Object, mapper.Object);

            var result = await handler.Handle(query, default);

            Assert.That(usersDtos, Is.EquivalentTo(result));

            repository.Verify(r => r.GetAllAsync(), Times.Once);
            mapper.Verify(m => m.Map<List<UserDTO>>(users), Times.Once);
        }
    }
}
