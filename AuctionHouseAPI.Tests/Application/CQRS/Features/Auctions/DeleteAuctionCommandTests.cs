using AuctionHouseAPI.Application.CQRS.Features.Auctions.Commands;
using AuctionHouseAPI.Application.CQRS.Features.Auctions.Handlers;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace AuctionHouseAPI.Tests.Application.CQRS.Features.Auctions
{
    [TestFixture]
    public class DeleteAuctionCommandTests
    {
        private Mock<IAuctionRepository> repository;
        private Mock<IAuctionService> service;
        private Mock<ILogger<DeleteAuctionHandler>> logger;
        private DeleteAuctionHandler handler;
        [SetUp]
        public void Setup()
        {
            repository = new Mock<IAuctionRepository>();
            service = new Mock<IAuctionService>();
            logger = new Mock<ILogger<DeleteAuctionHandler>>();
            handler = new DeleteAuctionHandler(service.Object, repository.Object, logger.Object);
        }
        [Test]
        public async Task ShouldFetchAuctionAndCallServiceIfItExists()
        {
            var auction = new Auction { Id = 1 };
            var command = new DeleteAuctionCommand(1, 1); 
            repository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(auction);
            service.Setup(s => s.DeleteAuctionAsync(auction, 1)).Returns(Task.CompletedTask);

            await handler.Handle(command, default);

            repository.Verify(r => r.GetByIdAsync(1), Times.Once);
            service.Verify(s => s.DeleteAuctionAsync(auction, 1), Times.Once);
        }
        [Test]
        public async Task ShouldThrowExceptionIfAuctionDoesNotExist()
        {
            var command = new DeleteAuctionCommand(1, 1);
            repository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Auction?)null);
            await Task.Delay(1);
            Assert.ThrowsAsync<EntityDoesNotExistException>(async () => await handler.Handle(command, default));
        }
    }
}
