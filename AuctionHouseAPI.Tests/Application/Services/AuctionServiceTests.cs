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
    public class AuctionServiceTests
    {
        private IAuctionService service;
        private Mock<IAuctionRepository> auctionRepository;
        [SetUp]
        public void Setup()
        {
            auctionRepository = new Mock<IAuctionRepository>();
            service = new AuctionService(auctionRepository.Object);
            auctionRepository.Setup(r => r.BeginTransactionAsync()).Returns(Task.CompletedTask);
            auctionRepository.Setup(r => r.CommitTransactionAsync()).Returns(Task.CompletedTask);
            auctionRepository.Setup(r => r.RollbackTransactionAsync()).Returns(Task.CompletedTask);
        }
        [Test]
        public async Task CreateAuctionShouldRetrunNewAuctionId()
        {
            var auction = new Auction { Id = 1 };
 
            auctionRepository.Setup(r => r.CreateAsync(auction)).ReturnsAsync(auction.Id);

            var result = await service.CreateAuctionAsync(auction);

            Assert.That(result, Is.EqualTo(auction.Id));
            auctionRepository.Verify(r => r.CreateAsync(auction), Times.Once);
        }
        [Test]
        public void AddTagsToAuctionShouldAddAuctionItemTagsToAuction()
        {
            var auction = new Auction
            {
                Item = new AuctionItem()
            };
            List<Tag> tags = new List<Tag>
            {
                new Tag { Id = 1 },
                new Tag { Id = 2 },
                new Tag { Id = 3 }
            };

            service.AddTagsToAuction(tags, auction);

            var addedTags = auction.Item.Tags.Select(t => t.TagId).ToList();
            var tagIds = tags.Select(t => t.Id).ToList();

            CollectionAssert.AreEquivalent(tagIds, addedTags);
        }
        [Test]
        public async Task DeleteAuctionShouldDeleteAuctionIfUserIsOwnerAndAuctionIsInactive()
        {
            var userId = 1;
            var auction = new Auction { Options = new AuctionOptions() };
            auction.Options.IsActive = false;
            auction.OwnerId = userId;
            auction.Options.FinishDateTime = DateTime.UtcNow.AddDays(1);

            auctionRepository.Setup(r => r.DeleteAsync(auction)).Returns(Task.CompletedTask);

            await service.DeleteAuctionAsync(auction, userId);

            auctionRepository.Verify(r => r.BeginTransactionAsync(), Times.Once);
            auctionRepository.Verify(r => r.CommitTransactionAsync(), Times.Once);
            auctionRepository.Verify(r => r.RollbackTransactionAsync(), Times.Never);
            auctionRepository.Verify(r => r.DeleteAsync(auction), Times.Once);
        }
        [Test]
        public async Task DeleteAuctionShouldThrowExceptionWhenUserIsNotTheOwner()
        {
            var userId = 1;
            var auction = new Auction { Options = new AuctionOptions() };
            auction.Options.IsActive = false;
            auction.OwnerId = 2;

            auctionRepository.Setup(r => r.DeleteAsync(auction)).Returns(Task.CompletedTask);
            await Task.Delay(1); // idk how to make it work without it
            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await service.DeleteAuctionAsync(auction, userId));
            auctionRepository.Verify(r => r.BeginTransactionAsync(), Times.Once);
            auctionRepository.Verify(r => r.CommitTransactionAsync(), Times.Never);
            auctionRepository.Verify(r => r.RollbackTransactionAsync(), Times.Once);
            auctionRepository.Verify(r => r.DeleteAsync(auction), Times.Never);
        }
        [Test]
        public async Task DeleteAuctionShouldThrowExceptionWhenAuctionIsActive()
        {
            var userId = 1;
            var auction = new Auction { Options = new AuctionOptions() };
            auction.Options.IsActive = true;
            auction.OwnerId = userId;

            auctionRepository.Setup(r => r.DeleteAsync(auction)).Returns(Task.CompletedTask);
            await Task.Delay(1); // idk how to make it work without it
            Assert.ThrowsAsync<ActiveAuctionException>(async () => await service.DeleteAuctionAsync(auction, userId));
            auctionRepository.Verify(r => r.BeginTransactionAsync(), Times.Once);
            auctionRepository.Verify(r => r.CommitTransactionAsync(), Times.Never);
            auctionRepository.Verify(r => r.RollbackTransactionAsync(), Times.Once);
            auctionRepository.Verify(r => r.DeleteAsync(auction), Times.Never);
        }
        [Test]
        public async Task DeleteAuctionShouldThrowExceptionWhenAuctionIsFinished()
        {
            var userId = 1;
            var auction = new Auction { Options = new AuctionOptions() };
            auction.Options.IsActive = false;
            auction.OwnerId = userId;
            auction.Options.FinishDateTime = DateTime.UtcNow.AddDays(-1);

            auctionRepository.Setup(r => r.DeleteAsync(auction)).Returns(Task.CompletedTask);
            await Task.Delay(1); // idk how to make it work without it
            Assert.ThrowsAsync<FinishedAuctionException>(async () => await service.DeleteAuctionAsync(auction, userId));
            auctionRepository.Verify(r => r.BeginTransactionAsync(), Times.Once);
            auctionRepository.Verify(r => r.CommitTransactionAsync(), Times.Never);
            auctionRepository.Verify(r => r.RollbackTransactionAsync(), Times.Once);
            auctionRepository.Verify(r => r.DeleteAsync(auction), Times.Never);
        }
        [Test]
        public async Task UpdateAuctionItemShouldUpdateItemIfUserIsOwnerAndAuctionIsInactive()
        {
            int userId = 1;
            var auction = new Auction { Options = new AuctionOptions(), Item = new AuctionItem() };
            auction.OwnerId = userId;
            auction.Options.IsActive = false;
            auction.Options.FinishDateTime = DateTime.UtcNow.AddDays(1);
            var updatedAuctionItem = new UpdateAuctionItemDTO("test", null, null, []);

            auctionRepository.Setup(r => r.UpdateAuctionItemAsync(auction.Item)).Returns(Task.CompletedTask);

            await service.UpdateAuctionItemAsync(auction, updatedAuctionItem, userId);

            Assert.That(auction.Item.Name, Is.EqualTo(updatedAuctionItem.Name));
            auctionRepository.Verify(r => r.BeginTransactionAsync(), Times.Once);
            auctionRepository.Verify(r => r.CommitTransactionAsync(), Times.Once);
            auctionRepository.Verify(r => r.RollbackTransactionAsync(), Times.Never);
            auctionRepository.Verify(r => r.UpdateAuctionItemAsync(auction.Item), Times.Once);
        }
        [Test]
        public async Task UpdateAuctionItemShouldThrowExceptionIfUserIsNotTheOwner()
        {
            int userId = 1;
            var auction = new Auction { Options = new AuctionOptions(), Item = new AuctionItem() };
            auction.OwnerId = 2;
            auction.Options.IsActive = false;
            auction.Options.FinishDateTime = DateTime.UtcNow.AddDays(1);
            var updatedAuctionItem = new UpdateAuctionItemDTO("test", null, null, []);

            auctionRepository.Setup(r => r.UpdateAuctionItemAsync(auction.Item)).Returns(Task.CompletedTask);

            await Task.Delay(1);
            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await service.UpdateAuctionItemAsync(auction, updatedAuctionItem, userId));

            auctionRepository.Verify(r => r.BeginTransactionAsync(), Times.Once);
            auctionRepository.Verify(r => r.CommitTransactionAsync(), Times.Never);
            auctionRepository.Verify(r => r.RollbackTransactionAsync(), Times.Once);
            auctionRepository.Verify(r => r.UpdateAuctionItemAsync(auction.Item), Times.Never);
        }
        [Test]
        public async Task UpdateAuctionItemShouldThrowExceptionIfAuctionIsActive()
        {
            int userId = 1;
            var auction = new Auction { Options = new AuctionOptions(), Item = new AuctionItem() };
            auction.OwnerId = userId;
            auction.Options.IsActive = true;
            auction.Options.FinishDateTime = DateTime.UtcNow.AddDays(1);
            var updatedAuctionItem = new UpdateAuctionItemDTO("test", null, null, []);

            auctionRepository.Setup(r => r.UpdateAuctionItemAsync(auction.Item)).Returns(Task.CompletedTask);

            await Task.Delay(1);
            Assert.ThrowsAsync<ActiveAuctionException>(async () => await service.UpdateAuctionItemAsync(auction, updatedAuctionItem, userId));

            auctionRepository.Verify(r => r.BeginTransactionAsync(), Times.Once);
            auctionRepository.Verify(r => r.CommitTransactionAsync(), Times.Never);
            auctionRepository.Verify(r => r.RollbackTransactionAsync(), Times.Once);
            auctionRepository.Verify(r => r.UpdateAuctionItemAsync(auction.Item), Times.Never);
        }
        [Test]
        public async Task UpdateAuctionItemShouldThrowExceptionIfAuctionIsFinished()
        {
            int userId = 1;
            var auction = new Auction { Options = new AuctionOptions(), Item = new AuctionItem() };
            auction.OwnerId = userId;
            auction.Options.IsActive = false;
            auction.Options.FinishDateTime = DateTime.UtcNow.AddDays(-1);
            var updatedAuctionItem = new UpdateAuctionItemDTO("test", null, null, []);

            auctionRepository.Setup(r => r.UpdateAuctionItemAsync(auction.Item)).Returns(Task.CompletedTask);

            await Task.Delay(1);
            Assert.ThrowsAsync<FinishedAuctionException>(async () => await service.UpdateAuctionItemAsync(auction, updatedAuctionItem, userId));

            auctionRepository.Verify(r => r.BeginTransactionAsync(), Times.Once);
            auctionRepository.Verify(r => r.CommitTransactionAsync(), Times.Never);
            auctionRepository.Verify(r => r.RollbackTransactionAsync(), Times.Once);
            auctionRepository.Verify(r => r.UpdateAuctionItemAsync(auction.Item), Times.Never);
        }
        [Test]
        public async Task UpdateAuctionOptionsShouldUpdateOptionsIfUserIsOwnerAndAuctionIsInactive()
        {
            int userId = 1;
            var auction = new Auction { Options = new AuctionOptions(), Item = new AuctionItem() };
            auction.OwnerId = userId;
            auction.Options.IsActive = false;
            auction.Options.FinishDateTime = DateTime.UtcNow.AddDays(1);
            var updatedAuctionOptions = new UpdateAuctionOptionsDTO(29.99M, null, null, null, null, null, null, null);

            auctionRepository.Setup(r => r.UpdateAuctionOptionsAsync(auction.Options)).Returns(Task.CompletedTask);

            await service.UpdateAuctionOptionsAsync(auction, updatedAuctionOptions, userId);

            Assert.That(auction.Options.StartingPrice, Is.EqualTo(updatedAuctionOptions.StartingPrice));
            auctionRepository.Verify(r => r.BeginTransactionAsync(), Times.Once);
            auctionRepository.Verify(r => r.CommitTransactionAsync(), Times.Once);
            auctionRepository.Verify(r => r.RollbackTransactionAsync(), Times.Never);
            auctionRepository.Verify(r => r.UpdateAuctionOptionsAsync(auction.Options), Times.Once);
        }
        [Test]
        public async Task UpdateAuctionOptionsShouldThrowExceptionIfUserIsNotTheOwner()
        {
            int userId = 1;
            var auction = new Auction { Options = new AuctionOptions(), Item = new AuctionItem() };
            auction.OwnerId = 2;
            auction.Options.IsActive = false;
            auction.Options.FinishDateTime = DateTime.UtcNow.AddDays(1);
            var updatedAuctionOptions = new UpdateAuctionOptionsDTO(29.99M, null, null, null, null, null, null, null);

            auctionRepository.Setup(r => r.UpdateAuctionOptionsAsync(auction.Options)).Returns(Task.CompletedTask);

            await Task.Delay(1);
            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await service.UpdateAuctionOptionsAsync(auction, updatedAuctionOptions, userId));

            auctionRepository.Verify(r => r.BeginTransactionAsync(), Times.Once);
            auctionRepository.Verify(r => r.CommitTransactionAsync(), Times.Never);
            auctionRepository.Verify(r => r.RollbackTransactionAsync(), Times.Once);
            auctionRepository.Verify(r => r.UpdateAuctionOptionsAsync(auction.Options), Times.Never);
        }
        [Test]
        public async Task UpdateAuctionOptionsShouldThrowExceptionIfAuctionIsActive()
        {
            int userId = 1;
            var auction = new Auction { Options = new AuctionOptions(), Item = new AuctionItem() };
            auction.OwnerId = userId;
            auction.Options.IsActive = true;
            auction.Options.FinishDateTime = DateTime.UtcNow.AddDays(1);
            var updatedAuctionOptions = new UpdateAuctionOptionsDTO(29.99M, null, null, null, null, null, null, null);

            auctionRepository.Setup(r => r.UpdateAuctionOptionsAsync(auction.Options)).Returns(Task.CompletedTask);

            await Task.Delay(1);
            Assert.ThrowsAsync<ActiveAuctionException>(async () => await service.UpdateAuctionOptionsAsync(auction, updatedAuctionOptions, userId));

            auctionRepository.Verify(r => r.UpdateAuctionOptionsAsync(auction.Options), Times.Never);
        }
        [Test]
        public async Task UpdateAuctionOptionsShouldThrowExceptionIfAuctionIsFinished()
        {
            int userId = 1;
            var auction = new Auction { Options = new AuctionOptions(), Item = new AuctionItem() };
            auction.OwnerId = userId;
            auction.Options.IsActive = false;
            auction.Options.FinishDateTime = DateTime.UtcNow.AddDays(-1);
            var updatedAuctionOptions = new UpdateAuctionOptionsDTO(29.99M, null, null, null, null, null, null, null);

            auctionRepository.Setup(r => r.UpdateAuctionOptionsAsync(auction.Options)).Returns(Task.CompletedTask);

            await Task.Delay(1);
            Assert.ThrowsAsync<FinishedAuctionException>(async () => await service.UpdateAuctionOptionsAsync(auction, updatedAuctionOptions, userId));

            auctionRepository.Verify(r => r.BeginTransactionAsync(), Times.Once);
            auctionRepository.Verify(r => r.CommitTransactionAsync(), Times.Never);
            auctionRepository.Verify(r => r.RollbackTransactionAsync(), Times.Once);
            auctionRepository.Verify(r => r.UpdateAuctionOptionsAsync(auction.Options), Times.Never);
        }
    }
}
