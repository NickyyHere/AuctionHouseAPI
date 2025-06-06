using AuctionHouseAPI.Application.CQRS.Features.Categories.Commands;
using AuctionHouseAPI.Application.CQRS.Features.Categories.Handlers;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;

namespace AuctionHouseAPI.Tests.Application.CQRS.Features.Categories
{
    [TestFixture]
    public class DeleteCategoryCommandTests
    {
        private Mock<ICategoryRepository> repository;
        private Mock<ICategoryService> service;
        private Mock<ILogger<DeleteCategoryHandler>> logger;
        private DeleteCategoryHandler handler;
        [SetUp]
        public void Setup()
        {
            repository = new Mock<ICategoryRepository>();
            service = new Mock<ICategoryService>();
            logger = new Mock<ILogger<DeleteCategoryHandler>>();
            handler = new DeleteCategoryHandler(repository.Object, service.Object, logger.Object);
        }
        [Test]
        public async Task ShouldCallRepositoryAndService()
        {
            var category = new Category();

            repository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);
            service.Setup(s => s.DeleteCategoryAsync(category)).Returns(Task.CompletedTask);

            var command = new DeleteCategoryCommand(1);
            await handler.Handle(command, default);

            repository.Verify(r => r.GetByIdAsync(1), Times.Once);
            service.Verify(s => s.DeleteCategoryAsync(category), Times.Once);
        }
        [Test]
        public void ShouldThrowExceptionIfCategoryDoesNotExist()
        {
            repository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Category?)null);

            var command = new DeleteCategoryCommand(1);
            Assert.ThrowsAsync<EntityDoesNotExistException>(() => handler.Handle(command, default));

            repository.Verify(r => r.GetByIdAsync(1), Times.Once);
        }
    }
}
