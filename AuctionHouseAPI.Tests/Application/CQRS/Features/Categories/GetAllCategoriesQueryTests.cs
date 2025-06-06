using AuctionHouseAPI.Application.CQRS.Features.Categories.Handlers;
using AuctionHouseAPI.Application.CQRS.Features.Categories.Queries;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AutoMapper;
using Moq;

namespace AuctionHouseAPI.Tests.Application.CQRS.Features.Categories
{
    public class GetAllCategoriesQueryTests
    {
        [Test]
        public async Task ShouldCallRepositoryAndMapToDTOs()
        {
            var repository = new Mock<ICategoryRepository>();
            var mapper = new Mock<IMapper>();

            var categories = new List<Category>
            {
                new Category(),
                new Category()
            };
            var categoriesDtos = new List<CategoryDTO>
            {
                new CategoryDTO("", ""),
                new CategoryDTO("", "")
            };

            repository.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);
            mapper.Setup(m => m.Map<List<CategoryDTO>>(categories)).Returns(categoriesDtos);

            var query = new GetAllCategoriesQuery();
            var handler = new GetAllCategoriesHandler(repository.Object, mapper.Object);

            var result = await handler.Handle(query, default);

            CollectionAssert.AreEquivalent(result, categoriesDtos);

            repository.Verify(r => r.GetAllAsync(), Times.Once);
            mapper.Verify(r => r.Map<List<CategoryDTO>>(categories), Times.Once);
        }
    }
}
