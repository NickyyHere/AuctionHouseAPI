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
    public class UpdateAuctionOptionsCommandTests
    {
        private Mock<IAuctionService> auctionService;
        private Mock<IAuctionRepository> auctionRepository;
        private Mock<ILogger<UpdateAuctionOptionsHandler>> logger;
        private UpdateAuctionOptionsHandler handler;
        [SetUp]
        public void Setup()
        {
            auctionService = new Mock<IAuctionService>();
            auctionRepository = new Mock<IAuctionRepository>();
            logger = new Mock<ILogger<UpdateAuctionOptionsHandler>>();
            handler = new UpdateAuctionOptionsHandler(auctionService.Object, auctionRepository.Object, logger.Object);
        }
        [Test]
        public async Task ShouldCallRepositoryAndServices()
        {
            var updateDto = new UpdateAuctionOptionsDTO(1, DateTime.UtcNow, DateTime.UtcNow, false, 1, 1, false, 1);
            var command = new UpdateAuctionOptionsCommand(updateDto, 1, 1);
            var auction = new Auction();
            auctionRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(auction);
            auctionService.Setup(s => s.UpdateAuctionOptionsAsync(auction, updateDto, 1)).Returns(Task.CompletedTask);

            await handler.Handle(command, default);

            auctionRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
            auctionService.Verify(s => s.UpdateAuctionOptionsAsync(auction, updateDto, 1), Times.Once);
        }
        [Test]
        public async Task ShouldThrowExceptionIfAuctionDoesNotExist()
        {
            var updateDto = new UpdateAuctionOptionsDTO(1, DateTime.UtcNow, DateTime.UtcNow, false, 1, 1, false, 1);
            var command = new UpdateAuctionOptionsCommand(updateDto, 1, 1);
            auctionRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Auction?)null);

            await Task.Delay(1);
            Assert.ThrowsAsync<EntityDoesNotExistException>(async () => await handler.Handle(command, default));

            auctionRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
        }
    }
}
