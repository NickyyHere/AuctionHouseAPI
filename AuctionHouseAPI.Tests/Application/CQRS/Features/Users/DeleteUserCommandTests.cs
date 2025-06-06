using AuctionHouseAPI.Application.CQRS.Features.Users.Commands;
using AuctionHouseAPI.Application.CQRS.Features.Users.Handlers;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace AuctionHouseAPI.Tests.Application.CQRS.Features.Users
{
    public class DeleteUserCommandTests
    {
        private Mock<IUserService> service;
        private Mock<IUserRepository> repository;
        private Mock<ILogger<DeleteUserHandler>> logger;
        private DeleteUserHandler handler;
        [SetUp]
        public void Setup()
        {
            service = new Mock<IUserService>();
            repository = new Mock<IUserRepository>();
            logger = new Mock<ILogger<DeleteUserHandler>>();
            handler = new DeleteUserHandler(service.Object, repository.Object, logger.Object);
        }
        [Test]
        public async Task ShouldCallRepositoryAndService()
        {
            var user = new User();

            repository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
            service.Setup(s => s.DeleteUserAsync(user)).Returns(Task.CompletedTask);

            var command = new DeleteUserCommand(1);

            await handler.Handle(command, default);

            repository.Verify(r => r.GetByIdAsync(1), Times.Once);
            service.Verify(s => s.DeleteUserAsync(user), Times.Once);
        }
        [Test]
        public void ShouldThrowExceptionIfUserDoesNotExist()
        {
            repository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((User?)null);

            var command = new DeleteUserCommand(1);

            Assert.ThrowsAsync<EntityDoesNotExistException>(() => handler.Handle(command, default));

            repository.Verify(r => r.GetByIdAsync(1), Times.Once);
        }
    }
}
