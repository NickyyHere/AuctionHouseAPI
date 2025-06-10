using AuctionHouseAPI.Application.CQRS.Features.Bids.Commands;
using AuctionHouseAPI.Application.CQRS.Features.Bids.Handlers;
using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Shared.Exceptions;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace AuctionHouseAPI.Tests.Application.CQRS.Features.Bids
{
    [TestFixture]
    public class CreateBidCommandTests
    {
        private Mock<IAuctionRepository> auctionRepository;
        private Mock<IBidService> bidsService;
        private Mock<IMapper> mapper;
        private Mock<ILogger<CreateBidHandler>> logger;
        private CreateBidHandler handler;

        [SetUp]
        public void Setup()
        {
            auctionRepository = new Mock<IAuctionRepository>();
            bidsService = new Mock<IBidService>();
            mapper = new Mock<IMapper>();
            logger = new Mock<ILogger<CreateBidHandler>>();
            handler = new CreateBidHandler(auctionRepository.Object, bidsService.Object, mapper.Object, logger.Object);
        }
        [Test]
        public async Task ShouldCallAuctionRepositoryMapToEntityAndCallService()
        {
            var command = new CreateBidCommand(new CreateBidDTO(1, 1), 1);
            var auction = new Auction();
            var bidDto = new BidDTO(1, 1, 1);
            var bid = new Bid();
            auctionRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(auction);
            mapper.Setup(m => m.Map<Bid>(command.CreateBidDTO)).Returns(bid);
            bidsService.Setup(s => s.CreateBidAsync(bid, auction, 1)).Returns(Task.CompletedTask);

            await handler.Handle(command, default);

            auctionRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
            mapper.Verify(m => m.Map<Bid>(command.CreateBidDTO), Times.Once);
            bidsService.Verify(s => s.CreateBidAsync(bid, auction, 1), Times.Once);
        }
        [Test]
        public void ShouldThrowExceptionIfEntityDoesNotExist()
        {
            var command = new CreateBidCommand(new CreateBidDTO(1, 1), 1);

            auctionRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Auction?)null);

            Assert.ThrowsAsync<EntityDoesNotExistException>(() => handler.Handle(command, default));

            auctionRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
        }
    }
}
