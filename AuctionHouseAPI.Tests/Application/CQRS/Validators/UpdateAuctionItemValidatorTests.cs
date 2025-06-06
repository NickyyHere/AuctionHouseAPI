using AuctionHouseAPI.Application.CQRS.Features.Auctions.Commands;
using AuctionHouseAPI.Application.CQRS.Validators;
using AuctionHouseAPI.Application.DTOs.Update;
using System.Xml.Linq;

namespace AuctionHouseAPI.Tests.Application.CQRS.Validators
{
    public class UpdateAuctionItemValidatorTests
    {
        private UpdateAuctionItemValidator validator = new();
        [TestCase("name", true)]
        [TestCase("", false)]
        public void ShouldValidateNameCorrectly(string name, bool expected)
        {
            var update = new UpdateAuctionItemDTO(name, "", 1, []);

            var result = validator.Validate(new UpdateAuctionItemCommand(update, 1, 1));

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
        [TestCase(0, false)]
        [TestCase(1, true)]
        public void ShouldValidateAuctionIdCorrectly(int id, bool expected)
        {
            var update = new UpdateAuctionItemDTO("sdf", "", 1, []);

            var result = validator.Validate(new UpdateAuctionItemCommand(update, id, 1));

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
    }
}
