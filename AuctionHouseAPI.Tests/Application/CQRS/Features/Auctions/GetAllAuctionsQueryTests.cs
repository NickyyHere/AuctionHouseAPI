using AuctionHouseAPI.Application.CQRS.Features.Auctions.Handlers;
using AuctionHouseAPI.Application.CQRS.Features.Auctions.Queries;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AutoMapper;
using Moq;
using System.Threading.Tasks;

namespace AuctionHouseAPI.Tests.Application.CQRS.Features.Auctions
{
    [TestFixture]
    public class GetAllAuctionsQueryTests
    {
        [Test]
        public async Task ShouldGetAllAuctionsAndMapToDTOs()
        {
            var repository = new Mock<IAuctionRepository>();
            var mapper = new Mock<IMapper>();
            var query = new GetAllAuctionsQuery();
            var handler = new GetAllAuctionsHandler(repository.Object, mapper.Object);

            var auctions = new List<Auction>
            {
                new Auction (),
                new Auction(),
                new Auction()
            };
            var auctionsDtos = new List<AuctionDTO>
            {
                new AuctionDTO(1, "", "", new AuctionItemDTO(1, 1, "", "", []), new AuctionOptionsDTO(1, 1, DateTime.UtcNow, DateTime.UtcNow, false, 0, 0, false, 0)),
                new AuctionDTO(2, "", "", new AuctionItemDTO(2, 1, "", "", []), new AuctionOptionsDTO(2, 1, DateTime.UtcNow, DateTime.UtcNow, false, 0, 0, false, 0)),
                new AuctionDTO(3, "", "", new AuctionItemDTO(3, 1, "", "", []), new AuctionOptionsDTO(3, 1, DateTime.UtcNow, DateTime.UtcNow, false, 0, 0, false, 0)),
            };

            repository.Setup(r => r.GetAllAsync()).ReturnsAsync(auctions);
            mapper.Setup(m => m.Map<List<AuctionDTO>>(auctions)).Returns(auctionsDtos);

            var result = await handler.Handle(query, default);

            CollectionAssert.AreEquivalent(result, auctionsDtos);
            repository.Verify(r => r.GetAllAsync(), Times.Once);
            mapper.Verify(m => m.Map<List<AuctionDTO>>(auctions), Times.Once);
        }
    }
}
