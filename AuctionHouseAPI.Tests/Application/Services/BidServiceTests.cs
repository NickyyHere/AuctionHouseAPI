using AuctionHouseAPI.Application.Services;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Shared.Exceptions;
using Moq;

namespace AuctionHouseAPI.Tests.Application.Services
{
    [TestFixture]
    public class BidServiceTests
    {
        private Mock<IBidRepository> bidRepository;
        private IBidService service;
        [SetUp]
        public void Setup()
        {
            bidRepository = new Mock<IBidRepository>();
            service = new BidService(bidRepository.Object);
            bidRepository.Setup(r => r.BeginTransactionAsync()).Returns(Task.CompletedTask);
            bidRepository.Setup(r => r.CommitTransactionAsync()).Returns(Task.CompletedTask);
            bidRepository.Setup(r => r.RollbackTransactionAsync()).Returns(Task.CompletedTask);
        }
        [Test]
        public async Task CreateBidShouldCreateBidIfAuctionIsActiveAndAmountIsSufficient()
        {
            var bid = new Bid { Amount = 300, AuctionId = 1 };
            var auctionOptions = new AuctionOptions { IsActive = true, MinimumOutbid = 1 };

            bidRepository.Setup(r => r.GetHighestAuctionBidAsync(1)).ReturnsAsync(new Bid { Amount = 100 });
            bidRepository.Setup(r => r.CreateAsync(bid)).ReturnsAsync(1);

            await service.CreateBidAsync(bid, auctionOptions, 1);

            bidRepository.Verify(r => r.GetHighestAuctionBidAsync(1), Times.Once);
            bidRepository.Verify(r => r.CreateAsync(bid), Times.Once);
        }
        [Test]
        public async Task CreateBidShouldThrowExceptionIfAuctionIsInactive()
        {
            var bid = new Bid { Amount = 300, AuctionId = 1 };
            var auctionOptions = new AuctionOptions { IsActive = false, MinimumOutbid = 1 };

            await Task.Delay(1);
            Assert.ThrowsAsync<InactiveAuctionException>(async () => await service.CreateBidAsync(bid, auctionOptions, 1));

            bidRepository.Verify(r => r.GetHighestAuctionBidAsync(1), Times.Never);
            bidRepository.Verify(r => r.CreateAsync(bid), Times.Never);
        }
        [Test]
        public async Task CreateBidShouldThrowExceptionIfBidIsBelowMinimumOutbid()
        {
            var bid = new Bid { Amount = 120, AuctionId = 1 };
            var auctionOptions = new AuctionOptions { IsActive = true, MinimumOutbid = 50 };

            bidRepository.Setup(r => r.GetHighestAuctionBidAsync(1)).ReturnsAsync(new Bid { Amount = 100 });

            await Task.Delay(1);
            Assert.ThrowsAsync<MinimumOutbidException>(async () => await service.CreateBidAsync(bid, auctionOptions, 1));

            bidRepository.Verify(r => r.GetHighestAuctionBidAsync(1), Times.Once);
            bidRepository.Verify(r => r.CreateAsync(bid), Times.Never);
        }
        [Test]
        public async Task WithdrawFromAuctionShouldDeleteAllUserBidsOnAuction()
        {
            Auction auction = new Auction { Id = 1, Options = new AuctionOptions() };
            var userId = 1;
            auction.Options.FinishDateTime = DateTime.UtcNow.AddDays(1);

            bidRepository.Setup(r => r.GetByUserAndAuctionAsync(1, 1)).ReturnsAsync(new List<Bid> { new Bid(), new Bid() });
            bidRepository.Setup(r => r.DeleteAsync(It.IsAny<Bid>())).Returns(Task.CompletedTask);

            await service.WithdrawFromAuctionAsync(auction, userId);

            bidRepository.Verify(r => r.BeginTransactionAsync(), Times.Once);
            bidRepository.Verify(r => r.CommitTransactionAsync(), Times.Once);
            bidRepository.Verify(r => r.RollbackTransactionAsync(), Times.Never);
            bidRepository.Verify(r => r.GetByUserAndAuctionAsync(1, 1), Times.Once);
            bidRepository.Verify(r => r.DeleteAsync(It.IsAny<Bid>()), Times.Exactly(2));
        }
        [Test]
        public async Task WithdrawFromAuctionShouldThrowExceptionIfAuctionIsFinished()
        {
            Auction auction = new Auction { Id = 1, Options = new AuctionOptions() };
            var userId = 1;
            auction.Options.FinishDateTime = DateTime.UtcNow.AddDays(-1);
            
            await Task.Delay(1);
            Assert.ThrowsAsync<FinishedAuctionException>(async () => await service.WithdrawFromAuctionAsync(auction, userId));

            bidRepository.Verify(r => r.BeginTransactionAsync(), Times.Once);
            bidRepository.Verify(r => r.CommitTransactionAsync(), Times.Never);
            bidRepository.Verify(r => r.RollbackTransactionAsync(), Times.Once);
        }
    }
}
