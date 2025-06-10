using AuctionHouseAPI.Application.CQRS.Features.Bids.Handlers;
using AuctionHouseAPI.Application.CQRS.Features.Bids.Queries;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AutoMapper;
using Moq;

namespace AuctionHouseAPI.Tests.Application.CQRS.Features.Bids
{
    public class GetAllUserBidsQueryTests
    {
        [Test]
        public async Task ShouldCallRepositoryAndMapToDTOs()
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
            repository.Setup(r => r.GetByUserAsync(1)).ReturnsAsync(bids);
            mapper.Setup(m => m.Map<List<BidDTO>>(bids)).Returns(bidsDtos);

            var query = new GetAllUserBidsQuery(1);
            var handler = new GetAllUserBidsHandler(repository.Object, mapper.Object);

            var result = await handler.Handle(query, default);

            Assert.That(result, Is.EquivalentTo(bidsDtos));

            repository.Verify(r => r.GetByUserAsync(1), Times.Once);
            mapper.Verify(m => m.Map<List<BidDTO>>(bids), Times.Once);
        }
    }
}
