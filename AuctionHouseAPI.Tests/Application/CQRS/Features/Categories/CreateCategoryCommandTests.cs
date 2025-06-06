using AuctionHouseAPI.Application.CQRS.Features.Categories.Commands;
using AuctionHouseAPI.Application.CQRS.Features.Categories.Handlers;
using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace AuctionHouseAPI.Tests.Application.CQRS.Features.Categories
{
    [TestFixture]
    public class CreateCategoryCommandTests
    {
        [Test]
        public async Task ShouldMapToEntityAndCallService()
        {
            var mapper = new Mock<IMapper>();
            var service = new Mock<ICategoryService>();
            var logger = new Mock<ILogger<CreateCategoryHandler>>();
            var createDto = new CreateCategoryDTO("", "");
            var category = new Category();

            mapper.Setup(m => m.Map<Category>(createDto)).Returns(category);
            service.Setup(s => s.CreateCategoryAsync(category)).ReturnsAsync(1);

            var command = new CreateCategoryCommand(createDto);
            var handler = new CreateCategoryHandler(service.Object, mapper.Object, logger.Object);

            var result = await handler.Handle(command, default);

            Assert.That(result, Is.EqualTo(1));

            mapper.Verify(m => m.Map<Category>(createDto), Times.Once);
            service.Verify(s => s.CreateCategoryAsync(category), Times.Once);
        }
    }
}
