using AuctionHouseAPI.Application.CQRS.Features.Categories.Handlers;
using AuctionHouseAPI.Application.CQRS.Features.Categories.Queries;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Shared.Exceptions;
using AutoMapper;
using Moq;

namespace AuctionHouseAPI.Tests.Application.CQRS.Features.Categories
{
    [TestFixture]
    public class GetCategoryByIdQueryTests
    {
        private Mock<ICategoryRepository> repository;
        private Mock<IMapper> mapper;
        private GetCategoryByIdHandler handler;
        [SetUp]
        public void Setup()
        {
            repository = new Mock<ICategoryRepository>();
            mapper = new Mock<IMapper>();
            handler = new GetCategoryByIdHandler(repository.Object, mapper.Object);
        }
        [Test]
        public async Task ShouldCallRepositoryAndMapToDTO()
        {
            var category = new Category();
            var categoryDTO = new CategoryDTO("", "");

            repository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);
            mapper.Setup(m => m.Map<CategoryDTO>(category)).Returns(categoryDTO);

            var query = new GetCategoryByIdQuery(1);

            var result = await handler.Handle(query, default);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(categoryDTO));

            repository.Verify(r => r.GetByIdAsync(1), Times.Once);
            mapper.Verify(m => m.Map<CategoryDTO>(category), Times.Once);
        }
        [Test]
        public void ShouldThrowExceptionIfCategoryDoesNotExist()
        {
            var category = new Category();

            repository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Category?)null);

            var query = new GetCategoryByIdQuery(1);

            Assert.ThrowsAsync<EntityDoesNotExistException>(() => handler.Handle(query, default));


            repository.Verify(r => r.GetByIdAsync(1), Times.Once);
        }
    }
}
