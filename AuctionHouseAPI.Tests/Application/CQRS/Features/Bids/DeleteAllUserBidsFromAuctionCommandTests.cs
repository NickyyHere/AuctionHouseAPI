using AuctionHouseAPI.Application.CQRS.Features.Bids.Commands;
using AuctionHouseAPI.Application.CQRS.Features.Bids.Handlers;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;

namespace AuctionHouseAPI.Tests.Application.CQRS.Features.Bids
{
    [TestFixture]
    public class DeleteAllUserBidsFromAuctionCommandTests
    {
        private Mock<IBidService> service;
        private Mock<IAuctionRepository> auctionRepository;
        private Mock<ILogger<DeleteAllUserBidsFromAuctionHandler>> logger;
        private DeleteAllUserBidsFromAuctionHandler handler;
        [SetUp]
        public void Setup()
        {
            service = new Mock<IBidService>();
            auctionRepository = new Mock<IAuctionRepository>();
            logger = new Mock<ILogger<DeleteAllUserBidsFromAuctionHandler>>();
            handler = new DeleteAllUserBidsFromAuctionHandler(auctionRepository.Object, service.Object, logger.Object);
        }
        [Test]
        public async Task ShouldCallAuctionRepositoryAndCallService()
        {
            var command = new DeleteAllUserBidsFromAuctionCommand(1, 1);
            var auction = new Auction();

            auctionRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(auction);
            service.Setup(s => s.WithdrawFromAuctionAsync(auction, 1)).Returns(Task.CompletedTask);

            await handler.Handle(command, default);

            auctionRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
            service.Verify(s => s.WithdrawFromAuctionAsync(auction, 1), Times.Once);
        }
        [Test]
        public void ShouldThrowExceptionIfAuctionDoesNotExist()
        {
            var command = new DeleteAllUserBidsFromAuctionCommand(1, 1);

            auctionRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Auction?)null);

            Assert.ThrowsAsync<EntityDoesNotExistException>(() => handler.Handle(command, default));

            auctionRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
        }
    }
}
