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
    public class GetAllAuctionBidsByUserQueryTests
    {
        [Test]
        public async Task ShouldCallRepositoryAndMapToDTO()
        {
            var repository = new Mock<IBidRepository>();
            var mapper = new Mock<IMapper>();
            var bids = new List<Bid>
            {
                new Bid(),
                new Bid()
            };
            var bidsDtos = new List<BidDTO>
            {
                new BidDTO(1,1,1),
                new BidDTO(2,2,2)
            };
            repository.Setup(r => r.GetByUserAndAuctionAsync(1, 1)).ReturnsAsync(bids);
            mapper.Setup(r => r.Map<List<BidDTO>>(bids)).Returns(bidsDtos);

            var query = new GetAllAuctionBidsByUserQuery(1, 1);
            var handler = new GetAllAuctionBidsByUserHandler(repository.Object, mapper.Object);

            var result = await handler.Handle(query, default);

            CollectionAssert.AreEquivalent(result, bidsDtos);

            repository.Verify(r => r.GetByUserAndAuctionAsync(1, 1), Times.Once);
            mapper.Verify(m => m.Map<List<BidDTO>>(bids), Times.Once);
        }
    }
}
