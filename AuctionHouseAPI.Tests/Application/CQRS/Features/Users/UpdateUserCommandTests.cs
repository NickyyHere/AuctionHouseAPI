using AuctionHouseAPI.Application.CQRS.Features.Users.Commands;
using AuctionHouseAPI.Application.CQRS.Features.Users.Handlers;
using AuctionHouseAPI.Application.DTOs.Update;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;

namespace AuctionHouseAPI.Tests.Application.CQRS.Features.Users
{
    public class UpdateUserCommandTests
    {
        private Mock<IUserRepository> repository;
        private Mock<IUserService> service;
        private Mock<ILogger<UpdateUserHandler>> logger;
        private UpdateUserHandler handler;
        [SetUp]
        public void Setup()
        {
            repository = new Mock<IUserRepository>();
            service = new Mock<IUserService>();
            logger = new Mock<ILogger<UpdateUserHandler>>();
            handler = new UpdateUserHandler(repository.Object, service.Object, logger.Object);
        }
        [Test]
        public async Task ShouldCallRepositoryAndService()
        {
            var user = new User();
            var updateDto = new UpdateUserDTO("", null, null, null);

            repository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
            service.Setup(s => s.UpdateUserAsync(user, updateDto)).Returns(Task.CompletedTask);

            var command = new UpdateUserCommand(updateDto, 1);

            await handler.Handle(command, default);

            repository.Verify(r => r.GetByIdAsync(1), Times.Once);
            service.Verify(s => s.UpdateUserAsync(user, updateDto), Times.Once);
        }
        [Test]
        public void ShouldThrowExceptionIfUserDoesNotExist()
        {
            var updateDto = new UpdateUserDTO("", null, null, null);

            repository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((User?)null);

            var command = new UpdateUserCommand(updateDto, 1);

            Assert.ThrowsAsync<EntityDoesNotExistException>(() => handler.Handle(command, default));

            repository.Verify(r => r.GetByIdAsync(1), Times.Once);
        }
    }
}
