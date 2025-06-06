using AuctionHouseAPI.Application.CQRS.Features.Categories.Commands;
using AuctionHouseAPI.Application.CQRS.Features.Categories.Handlers;
using AuctionHouseAPI.Application.DTOs.Update;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;

namespace AuctionHouseAPI.Tests.Application.CQRS.Features.Categories
{
    public class EditCategoryCommandTests
    {
        private Mock<ICategoryRepository> repository;
        private Mock<ICategoryService> service;
        private Mock<ILogger<EditCategoryHandler>> logger;
        private EditCategoryHandler handler;
        [SetUp]
        public void Setup()
        {
            repository = new Mock<ICategoryRepository>();
            service = new Mock<ICategoryService>();
            logger = new Mock<ILogger<EditCategoryHandler>>();
            handler = new EditCategoryHandler(repository.Object, service.Object, logger.Object);
        }
        [Test]
        public async Task ShouldCallRepositoryAndService()
        {
            var category = new Category();
            var updateDto = new UpdateCategoryDTO("", "");
            repository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);
            service.Setup(s => s.UpdateCategoryAsync(category, updateDto)).Returns(Task.CompletedTask);

            var command = new EditCategoryCommand(updateDto, 1);

            await handler.Handle(command, default);

            repository.Verify(r => r.GetByIdAsync(1), Times.Once);
            service.Verify(s => s.UpdateCategoryAsync(category, updateDto), Times.Once);
        }
        [Test]
        public void ShouldThrowExceptionIfCategoryDoesNotExist()
        {
            var category = new Category();
            var updateDto = new UpdateCategoryDTO("", "");
            repository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Category?)null);

            var command = new EditCategoryCommand(updateDto, 1);

            Assert.ThrowsAsync<EntityDoesNotExistException>(() => handler.Handle(command, default));

            repository.Verify(r => r.GetByIdAsync(1), Times.Once);
        }
    }
}
