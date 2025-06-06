using AuctionHouseAPI.Application.CQRS.Features.Auctions.Handlers;
using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace AuctionHouseAPI.Tests.Application.CQRS.Features.Auctions
{
    [TestFixture]
    public class CreateAuctionCommandTests
    {
        [Test]
        public async Task CreateAuctionHandlerShouldMapAddTagsAndCallServiceAsync()
        {
            var auction = new Auction { Id = 1 };
            var dto = new CreateAuctionDTO(
                new CreateAuctionItemDTO("test", "test", 1, []),
                new CreateAuctionOptionsDTO(0, DateTime.UtcNow, DateTime.UtcNow, false, 0, 0, false, 0)
                );

            var mapper = new Mock<IMapper>();
            var tagService = new Mock<ITagService>();
            var auctionService = new Mock<IAuctionService>();
            var logger = new Mock<ILogger<CreateAuctionHandler>>();

            mapper.Setup(m => m.Map<Auction>(dto)).Returns(auction);
            tagService.Setup(t => t.EnsureTagsExistAsync(It.IsAny<List<string>>())).ReturnsAsync([]);
            auctionService.Setup(a => a.AddTagsToAuction(It.IsAny<List<Tag>>(), auction)).Verifiable();
            auctionService.Setup(a => a.CreateAuctionAsync(auction)).ReturnsAsync(auction.Id);

            var handler = new CreateAuctionHandler(auctionService.Object, tagService.Object, mapper.Object, logger.Object);

            var result = await handler.Handle(new AuctionHouseAPI.Application.CQRS.Features.Auctions.Commands.CreateAuctionCommand(dto, 1), default);

            Assert.That(result, Is.EqualTo(auction.Id));

            mapper.Verify(m => m.Map<Auction>(dto), Times.Once);
            tagService.Verify(t => t.EnsureTagsExistAsync(It.IsAny<List<string>>()), Times.Once);
            auctionService.Verify(a => a.AddTagsToAuction(It.IsAny<List<Tag>>(), auction), Times.Once);
            auctionService.Verify(a => a.CreateAuctionAsync(auction), Times.Once);
        }
    }
}
