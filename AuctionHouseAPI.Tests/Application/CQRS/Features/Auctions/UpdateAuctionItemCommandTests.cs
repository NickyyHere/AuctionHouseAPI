using AuctionHouseAPI.Application.CQRS.Features.Auctions.Commands;
using AuctionHouseAPI.Application.CQRS.Features.Auctions.Handlers;
using AuctionHouseAPI.Application.DTOs.Update;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace AuctionHouseAPI.Tests.Application.CQRS.Features.Auctions
{
    public class UpdateAuctionItemCommandTests
    {
        private Mock<IAuctionService> auctionService;
        private Mock<ITagService> tagService;
        private Mock<IAuctionRepository> auctionRepository;
        private Mock<ILogger<UpdateAuctionItemHandler>> logger;
        private UpdateAuctionItemHandler handler;
        [SetUp]
        public void Setup()
        {
            auctionService = new Mock<IAuctionService>();
            tagService = new Mock<ITagService>();
            auctionRepository = new Mock<IAuctionRepository>();
            logger = new Mock<ILogger<UpdateAuctionItemHandler>>();
            handler = new UpdateAuctionItemHandler(auctionService.Object, auctionRepository.Object, tagService.Object, logger.Object);
        }
        [Test]
        public async Task ShouldCallRepositoryAndServices()
        {
            var updateDto = new UpdateAuctionItemDTO("", "", 1, []);
            var command = new UpdateAuctionItemCommand(updateDto, 1, 1);
            var auction = new Auction();
            auctionRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(auction);
            tagService.Setup(s => s.EnsureTagsExistAsync(It.IsAny<List<string>>())).ReturnsAsync([]);
            auctionService.Setup(s => s.UpdateAuctionItemAsync(auction, updateDto, 1)).Returns(Task.CompletedTask);

            await handler.Handle(command, default);

            auctionRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
            tagService.Verify(s => s.EnsureTagsExistAsync(It.IsAny<List<string>>()),Times.Once);
            auctionService.Verify(s => s.UpdateAuctionItemAsync(auction, updateDto, 1), Times.Once);
        }
        [Test]
        public async Task ShouldThrowExceptionIfAuctionDoesNotExist()
        {
            var updateDto = new UpdateAuctionItemDTO("", "", 1, []);
            var command = new UpdateAuctionItemCommand(updateDto, 1, 1);
            auctionRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Auction?)null);

            await Task.Delay(1);
            Assert.ThrowsAsync<EntityDoesNotExistException>(async () => await handler.Handle(command, default));

            auctionRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
        }
    }
}
