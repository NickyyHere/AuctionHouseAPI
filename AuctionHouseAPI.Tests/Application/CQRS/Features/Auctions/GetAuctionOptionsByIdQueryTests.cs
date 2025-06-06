using AuctionHouseAPI.Application.CQRS.Features.Auctions.Handlers;
using AuctionHouseAPI.Application.CQRS.Features.Auctions.Queries;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Shared.Exceptions;
using AutoMapper;
using Moq;

namespace AuctionHouseAPI.Tests.Application.CQRS.Features.Auctions
{
    public class GetAuctionOptionsByIdQueryTests
    {
        private Mock<IAuctionRepository> repository;
        private Mock<IMapper> mapper;
        private GetAuctionOptionsByIdHandler handler;

        [SetUp]
        public void Setup()
        {
            repository = new Mock<IAuctionRepository>();
            mapper = new Mock<IMapper>();
            handler = new GetAuctionOptionsByIdHandler(repository.Object, mapper.Object);
        }

        [Test]
        public async Task ShouldGetAuctionAndMapToDTO()
        {
            var query = new GetAuctionOptionsByIdQuery(1);
            var auction = new Auction();
            var auctionDTO = new AuctionDTO(1, "", "", new AuctionItemDTO(1, 1, "", "", []), new AuctionOptionsDTO(1, 1, DateTime.UtcNow, DateTime.UtcNow, false, 1, 1, false, 1));
            repository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(auction);
            mapper.Setup(m => m.Map<AuctionDTO>(auction)).Returns(auctionDTO);

            var result = await handler.Handle(query, default);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.EqualTo(auctionDTO.Options));
            }
            );
            repository.Verify(r => r.GetByIdAsync(1), Times.Once);
            mapper.Verify(m => m.Map<AuctionDTO>(auction), Times.Once);
        }
        [Test]
        public async Task ShouldThrowExceptionIfAuctionDoesNotExistAsync()
        {
            var query = new GetAuctionOptionsByIdQuery(1);

            repository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Auction?)null);

            await Task.Delay(1);
            Assert.ThrowsAsync<EntityDoesNotExistException>(async () => await handler.Handle(query, default));
        }
    }
}
