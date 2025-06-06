using AuctionHouseAPI.Application.CQRS.Features.Auctions.Handlers;
using AuctionHouseAPI.Application.CQRS.Features.Auctions.Queries;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AutoMapper;
using Moq;

namespace AuctionHouseAPI.Tests.Application.CQRS.Features.Auctions
{
    public class GetAllAuctionsByTagsQueryTests
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
        public async Task ShouldGetAuctionsForEachTagAndMapUniqueToDTOs()
        {
            var query = new GetAllAuctionsByTagsQuery(["t1", "t2"]);
            var auctions = new List<Auction>
            {
                new Auction(),
                new Auction(),
                new Auction(),
            };
            var dto = new AuctionDTO(3, "", "", new AuctionItemDTO(3, 1, "", "", []), new AuctionOptionsDTO(3, 1, DateTime.UtcNow, DateTime.UtcNow, false, 0, 0, false, 0));


            repository.Setup(r => r.GetByTagAsync(It.IsAny<string>())).ReturnsAsync(auctions);
            mapper.Setup(m => m.Map<AuctionDTO>(It.IsAny<Auction>())).Returns(dto);

            var handler = new GetAllAuctionsByTagsHandler(repository.Object, mapper.Object);    

            var result = await handler.Handle(query, default);

            Assert.True(result.All(r => r == dto));
            Assert.That(result.Count, Is.EqualTo(1));
            repository.Verify(r => r.GetByTagAsync(It.IsAny<string>()), Times.Exactly(2));
        }
    }
}
