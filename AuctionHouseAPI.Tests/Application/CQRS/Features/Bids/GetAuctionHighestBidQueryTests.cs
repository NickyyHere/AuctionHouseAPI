using AuctionHouseAPI.Application.CQRS.Features.Bids.Handlers;
using AuctionHouseAPI.Application.CQRS.Features.Bids.Queries;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AutoMapper;
using Moq;

namespace AuctionHouseAPI.Tests.Application.CQRS.Features.Bids
{
    [TestFixture]
    public class GetAuctionHighestBidQueryTests
    {
        [Test]
        public async Task ShouldCallRepositoryAndMapToDTOs()
        {
            var repository = new Mock<IBidRepository>();
            var mapper = new Mock<IMapper>();
            var bid = new Bid();
            var bidDto = new BidDTO(1, 1, 1);
            repository.Setup(r => r.GetHighestAuctionBidAsync(1)).ReturnsAsync(bid);
            mapper.Setup(m => m.Map<BidDTO>(bid)).Returns(bidDto);
            var query = new GetAuctionHighestBidQuery(1);
            var handler = new GetAuctionHighestBidHandler(repository.Object, mapper.Object);

            var result = await handler.Handle(query, default);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(bidDto));
        }
    }
}
