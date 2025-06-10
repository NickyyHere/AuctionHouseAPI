using AuctionHouseAPI.Application.CQRS.Features.Users.Handlers;
using AuctionHouseAPI.Application.CQRS.Features.Users.Queries;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Shared.Exceptions;
using AutoMapper;
using Moq;

namespace AuctionHouseAPI.Tests.Application.CQRS.Features.Users
{
    public class GetUserByIdQueryTest
    {
        private Mock<IUserRepository> repository;
        private Mock<IMapper> mapper;
        private GetUserByIdHandler handler;
        [SetUp]
        public void Setup()
        {
            repository = new Mock<IUserRepository>();
            mapper = new Mock<IMapper>();
            handler = new GetUserByIdHandler(repository.Object, mapper.Object);
        }
        [Test]
        public async Task ShouldCallRepositoryAndMapToDTO()
        {
            var user = new User();
            var userDTO = new UserDTO(1, "", "", "", DateTime.Now, Domain.Enums.UserRole.ROLE_USER);

            repository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
            mapper.Setup(m => m.Map<UserDTO>(user)).Returns(userDTO);

            var query = new GetUserByIdQuery(1);

            var result = await handler.Handle(query, default);

            Assert.That(result, Is.EqualTo(userDTO));

            repository.Verify(r => r.GetByIdAsync(1), Times.Once);
            mapper.Verify(m => m.Map<UserDTO>(user), Times.Once);
        }
        [Test]
        public void ShouldThrowExceptionWhenUserDoesNotExist()
        {
            repository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((User?)null);

            var query = new GetUserByIdQuery(1);

            Assert.ThrowsAsync<EntityDoesNotExistException>(() => handler.Handle(query, default));

            repository.Verify(r => r.GetByIdAsync(1), Times.Once);
        }
    }
}
