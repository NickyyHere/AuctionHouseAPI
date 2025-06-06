using AuctionHouseAPI.Application.CQRS.Features.Auctions.Handlers;
using AuctionHouseAPI.Application.CQRS.Features.Auctions.Queries;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AutoMapper;
using Moq;

namespace AuctionHouseAPI.Tests.Application.CQRS.Features.Auctions
{
    public class GetAllAuctionsByUserQueryTests
    {
        private Mock<IAuctionRepository> repository;
        private Mock<IMapper> mapper;
        [SetUp]
        public void Setup()
        {
            repository = new Mock<IAuctionRepository>();
            mapper = new Mock<IMapper>();
        }
        [Test]
        public async Task ShouldGetAuctionsAndMapToDTOs()
        {
            var auctions = new List<Auction>
            {
                new Auction { Item = new AuctionItem{ AuctionId = 1}},
                new Auction { Item = new AuctionItem{ AuctionId = 2}},
                new Auction { Item = new AuctionItem{ AuctionId = 3}}
            };
            var auctionsDtos = new List<AuctionDTO>
            {
                new AuctionDTO(1, "", "", new AuctionItemDTO(1, 1, "", "", []), new AuctionOptionsDTO(1, 1, DateTime.UtcNow, DateTime.UtcNow, false, 0, 0, false, 0)),
                new AuctionDTO(2, "", "", new AuctionItemDTO(2, 1, "", "", []), new AuctionOptionsDTO(2, 1, DateTime.UtcNow, DateTime.UtcNow, false, 0, 0, false, 0)),
                new AuctionDTO(3, "", "", new AuctionItemDTO(3, 1, "", "", []), new AuctionOptionsDTO(3, 1, DateTime.UtcNow, DateTime.UtcNow, false, 0, 0, false, 0)),
            };
            var query = new GetAllAuctionsByUserQuery(1);

            repository.Setup(r => r.GetByUserAsync(1)).ReturnsAsync(auctions);
            mapper.Setup(m => m.Map<List<AuctionDTO>>(auctions)).Returns(auctionsDtos);

            var handler = new GetAllAuctionsByUserHandler(repository.Object, mapper.Object);

            var result = await handler.Handle(query, default);

            CollectionAssert.AreEquivalent(auctionsDtos, result);
            repository.Verify(r => r.GetByUserAsync(1), Times.Once);
            mapper.Verify(m => m.Map<List<AuctionDTO>>(auctions), Times.Once);
        }
    }
}
