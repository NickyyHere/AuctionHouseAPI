using AuctionHouseAPI.Application.DTOs.Update;
using AuctionHouseAPI.Application.Services;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using Moq;

namespace AuctionHouseAPI.Tests.Application.Services
{
    [TestFixture]
    public class CategoryServiceTest
    {
        private Mock<ICategoryRepository> repository;
        private ICategoryService service;
        [SetUp]
        public void Setup()
        {
            repository = new Mock<ICategoryRepository>();
            service = new CategoryService(repository.Object);
        }
        [Test]
        public async Task CreateCategoryShouldReturnCategoryId()
        {
            var category = new Category { Id = 1 };

            repository.Setup(r => r.CreateAsync(category)).ReturnsAsync(category.Id);

            var result = await service.CreateCategoryAsync(category);

            Assert.That(category.Id, Is.EqualTo(result));
            repository.Verify(r => r.CreateAsync(category), Times.Once);
        }
        [Test]
        public async Task DeleteCategoryShouldCallRepositoryDeleteMethod()
        {
            var category = new Category();

            repository.Setup(r => r.DeleteAsync(category)).Returns(Task.CompletedTask);

            await service.DeleteCategoryAsync(category);

            repository.Verify(r => r.DeleteAsync(category), Times.Once);
        }
        [Test]
        public async Task UpdateCategoryShouldChangeCategoryFieldValues()
        {
            var category = new Category { Name = "Init", Description = "Init" };
            var updateDto = new UpdateCategoryDTO("New", "");

            repository.Setup(r => r.BeginTransactionAsync()).Returns(Task.CompletedTask);
            repository.Setup(r => r.CommitTransactionAsync()).Returns(Task.CompletedTask);
            repository.Setup(r => r.UpdateCategoryAsync(category)).Returns(Task.CompletedTask);

            await service.UpdateCategoryAsync(category, updateDto);

            Assert.Multiple(() =>
            {
                Assert.That(category.Name, Is.EqualTo("New"));
                Assert.That(category.Description, Is.EqualTo("Init"));
            });

            repository.Verify(r => r.BeginTransactionAsync(), Times.Once);
            repository.Verify(r => r.CommitTransactionAsync(), Times.Once);
            repository.Verify(r => r.UpdateCategoryAsync(category), Times.Once);
        }
    }
}
