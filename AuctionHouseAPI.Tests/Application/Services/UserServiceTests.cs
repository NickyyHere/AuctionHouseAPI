using AuctionHouseAPI.Application.DTOs.Update;
using AuctionHouseAPI.Application.Services;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Shared.Exceptions;
using Moq;

namespace AuctionHouseAPI.Tests.Application.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> repository;
        private IUserService service;
        [SetUp]
        public void Setup()
        {
            repository = new Mock<IUserRepository>();
            service = new UserService(repository.Object);
        }
        [Test]
        public async Task CreateUserShouldReturnIdWhenUsernameIsNotUsed()
        {
            var user = new User { Id = 1 ,Username = "Test" };

            repository.Setup(r => r.CreateAsync(user)).ReturnsAsync(user.Id);
            repository.Setup(r => r.GetByUsernameAsync(user.Username)).ReturnsAsync((User?)null);

            var result = await service.CreateUserAsync(user);
            
            Assert.That(result, Is.EqualTo(user.Id));
            repository.Verify(r => r.CreateAsync(user), Times.Once);
            repository.Verify(r => r.GetByUsernameAsync(user.Username), Times.Once);
        }
        [Test]
        public async Task CreateUserShouldThrowExceptionWhenUsernameIsTaken()
        {
            var user = new User { Id = 1, Username = "Test" };

            repository.Setup(r => r.GetByUsernameAsync(user.Username)).ReturnsAsync(user);

            await Task.Delay(1);
            Assert.ThrowsAsync<DuplicateEntityException>(async () => await service.CreateUserAsync(user));

            repository.Verify(r => r.GetByUsernameAsync(user.Username), Times.Once);
        }
        [Test]
        public async Task DeleteUserShouldCallRepositoryMethod()
        {
            var user = new User();

            repository.Setup(r => r.DeleteAsync(user)).Returns(Task.CompletedTask);

            await service.DeleteUserAsync(user);

            repository.Verify(r => r.DeleteAsync(user), Times.Once);
        }
        [Test]
        public async Task UpdateUserShouldUpdateFieldValues()
        {
            var user = new User { FirstName = "Init", LastName = "Init" };
            var updateDto = new UpdateUserDTO(null, null, "New", "");

            repository.Setup(r => r.BeginTransactionAsync()).Returns(Task.CompletedTask);
            repository.Setup(r => r.CommitTransactionAsync()).Returns(Task.CompletedTask);
            repository.Setup(r => r.UpdateUserAsync(user)).Returns(Task.CompletedTask);

            await service.UpdateUserAsync(user, updateDto);

            Assert.Multiple(() =>
            {
                Assert.That(user.FirstName, Is.EqualTo("New"));
                Assert.That(user.LastName, Is.EqualTo("Init"));
            });

            repository.Verify(r => r.BeginTransactionAsync(), Times.Once);
            repository.Verify(r => r.CommitTransactionAsync(), Times.Once);
            repository.Verify(r => r.UpdateUserAsync(user), Times.Once);
        }
    }
}
