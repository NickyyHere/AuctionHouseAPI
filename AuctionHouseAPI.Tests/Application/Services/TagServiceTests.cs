using AuctionHouseAPI.Application.Services;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using Moq;

namespace AuctionHouseAPI.Tests.Application.Services
{
    [TestFixture]
    public class TagServiceTests
    {
        private Mock<ITagRepository> repository;
        private ITagService service;
        [SetUp]
        public void Setup()
        {
            repository = new Mock<ITagRepository>();
            service = new TagService(repository.Object);
        }
        [Test]
        public async Task CreateTagShouldRetrunTagId()
        {
            var tag = new Tag { Id = 1 };

            repository.Setup(r => r.CreateAsync(tag)).ReturnsAsync(tag.Id);

            var result = await service.CreateTag(tag);

            Assert.That(result, Is.EqualTo(tag.Id));
            repository.Verify(r => r.CreateAsync(tag), Times.Once);
        }
        [Test]
        public async Task EnsureTagsExistShouldCreateNewTagsIfTheyDontExist()
        {
            var tags = new List<Tag> 
            { 
                new Tag { Id = 1, Name = "tag1" },
                new Tag { Id = 2, Name = "tag2" },
                new Tag { Id = 3, Name = "tag3" }
            };

            int i = 0;

            repository.Setup(r => r.BeginTransactionAsync()).Returns(Task.CompletedTask);
            repository.Setup(r => r.CommitTransactionAsync()).Returns(Task.CompletedTask);
            repository.Setup(r => r.CreateAsync(It.IsAny<Tag>())).ReturnsAsync(() => ++i);
            repository.Setup(r => r.GetByNameAsync(It.IsAny<string>())).ReturnsAsync((Tag?)null);
            repository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => tags.First(t => t.Id == id));

            var result = await service.EnsureTagsExistAsync(tags.Select(t => t.Name).ToList());

            CollectionAssert.AreEquivalent(tags, result);
            repository.Verify(r => r.BeginTransactionAsync(), Times.Once);
            repository.Verify(r => r.CommitTransactionAsync(), Times.Once);
            repository.Verify(r => r.CreateAsync(It.IsAny<Tag>()), Times.Exactly(3));
            repository.Verify(r => r.GetByNameAsync(It.IsAny<string>()), Times.Exactly(3));
            repository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Exactly(3));

        }
        [Test]
        public async Task EnsureTagsExistShouldNotCreateNewTagIfItExists()
        {
            var tags = new List<Tag>
            {
                new Tag { Id = 1, Name = "tag1" },
                new Tag { Id = 2, Name = "tag2" },
                new Tag { Id = 3, Name = "tag3" }
            };

            int i = 0;

            repository.Setup(r => r.BeginTransactionAsync()).Returns(Task.CompletedTask);
            repository.Setup(r => r.CommitTransactionAsync()).Returns(Task.CompletedTask);
            repository.Setup(r => r.GetByNameAsync(It.IsAny<string>())).ReturnsAsync((string name) => tags.First(t => t.Name == name));

            var result = await service.EnsureTagsExistAsync(tags.Select(t => t.Name).ToList());

            CollectionAssert.AreEquivalent(tags, result);
            repository.Verify(r => r.BeginTransactionAsync(), Times.Once);
            repository.Verify(r => r.CommitTransactionAsync(), Times.Once);
            repository.Verify(r => r.CreateAsync(It.IsAny<Tag>()), Times.Never);
            repository.Verify(r => r.GetByNameAsync(It.IsAny<string>()), Times.Exactly(3));
            repository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }
    }
}
