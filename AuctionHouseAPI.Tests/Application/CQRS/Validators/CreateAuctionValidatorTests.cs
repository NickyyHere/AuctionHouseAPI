using AuctionHouseAPI.Application.CQRS.Features.Auctions.Commands;
using AuctionHouseAPI.Application.CQRS.Validators;
using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Domain.Models;
using System.Xml.Linq;

namespace AuctionHouseAPI.Tests.Application.CQRS.Validators
{
    [TestFixture]
    public class CreateAuctionValidatorTests
    {
        private CreateAuctionValidator auctionValidator = new();
        private CreateAuctionItemValidator itemValidator = new();
        private CreateAuctionOptionsValidator optionsValidator = new();

        [TestCase("correct", true)]
        [TestCase("", false)]
        [TestCase("c", false)]
        public void ShouldValidateItemNameCorrectly(string name, bool expected)
        {
            var item = new CreateAuctionItemDTO(name, "correct", 1, []);
            var options = new CreateAuctionOptionsDTO(20, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2), true, 5, 20, true, 200);
            var auction = new CreateAuctionDTO(item, options);

            var result = itemValidator.Validate(new CreateAuctionCommand(auction, 1));

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
        [TestCase("correct", true)]
        [TestCase("", false)]
        public void ShouldValidateItemDescriptionCorrectly(string description, bool expected)
        {
            var item = new CreateAuctionItemDTO("correct", description, 1, []);
            var options = new CreateAuctionOptionsDTO(20, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2), true, 5, 20, true, 200);
            var auction = new CreateAuctionDTO(item, options);

            var result = itemValidator.Validate(new CreateAuctionCommand(auction, 1));

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
        [TestCase(1, true)]
        [TestCase(0, false)]
        public void ShouldValidateItemCategoryCorrectly(int categoryId, bool expected)
        {
            var item = new CreateAuctionItemDTO("correct", "correct", categoryId, []);
            var options = new CreateAuctionOptionsDTO(20, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2), true, 5, 20, true, 200);
            var auction = new CreateAuctionDTO(item, options);

            var result = itemValidator.Validate(new CreateAuctionCommand(auction, 1));

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
        [TestCase(1, true)]
        [TestCase(0, false)]
        public void ShouldValidateOptionsStartingPriceCorrectly(decimal price, bool expected)
        {
            var item = new CreateAuctionItemDTO("correct", "correct", 1, []);
            var options = new CreateAuctionOptionsDTO(price, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2), true, 5, 20, true, 200);
            var auction = new CreateAuctionDTO(item, options);

            var result = optionsValidator.Validate(new CreateAuctionCommand(auction, 1));

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }

        private static readonly object[] StartDates =
        {
            new object[] { DateTime.UtcNow.AddDays(1), true },
            new object[] { DateTime.UtcNow.AddDays(-1), false },
        };
        [TestCaseSource(nameof(StartDates))]
        public void ShouldValidateOptionsStartDateCorrectly(DateTime startDate, bool expected)
        {
            var item = new CreateAuctionItemDTO("correct", "correct", 1, []);
            var options = new CreateAuctionOptionsDTO(20, startDate, DateTime.UtcNow.AddDays(2), true, 5, 20, true, 200);
            var auction = new CreateAuctionDTO(item, options);

            var result = optionsValidator.Validate(new CreateAuctionCommand(auction, 1));

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
        private static readonly object[] EndDates =
        {
            new object[] { DateTime.UtcNow.AddDays(2), true },
            new object[] { DateTime.UtcNow.AddDays(-2), false },
        };
        [TestCaseSource(nameof(EndDates))]
        public void ShouldValidateOptionsFinishDateCorrectly(DateTime endDate, bool expected)
        {
            var item = new CreateAuctionItemDTO("correct", "correct", 1, []);
            var options = new CreateAuctionOptionsDTO(20, DateTime.UtcNow, endDate, true, 5, 20, true, 200);
            var auction = new CreateAuctionDTO(item, options);

            var result = optionsValidator.Validate(new CreateAuctionCommand(auction, 1));

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
        [TestCase(5, true)]
        [TestCase(4, false)]
        public void ShouldValidateOptionsMinimumOutbidCorrectly(int minimumOutbid, bool expected)
        {
            var item = new CreateAuctionItemDTO("correct", "correct", 1, []);
            var options = new CreateAuctionOptionsDTO(20, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), true, 5, minimumOutbid, true, 200);
            var auction = new CreateAuctionDTO(item, options);

            var result = optionsValidator.Validate(new CreateAuctionCommand(auction, 1));

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
        [TestCase(10, false)]
        [TestCase(20, true)]
        public void ShouldValidateOptionsBINPriceCorrectly(int buyItNowPrice, bool expected)
        {
            var item = new CreateAuctionItemDTO("correct", "correct", 1, []);
            var options = new CreateAuctionOptionsDTO(15, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), true, 5, 5, true, buyItNowPrice);
            var auction = new CreateAuctionDTO(item, options);

            var result = optionsValidator.Validate(new CreateAuctionCommand(auction, 1));

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
        [TestCase(1, true)]
        [TestCase(0, false)]
        public void ShouldValidateUserIdCorrectly(int userId, bool expected)
        {
            var item = new CreateAuctionItemDTO("correct", "correct", 1, []);
            var options = new CreateAuctionOptionsDTO(15, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), true, 5, 5, true, 25);
            var auction = new CreateAuctionDTO(item, options);

            var result = auctionValidator.Validate(new CreateAuctionCommand(auction, userId));

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
        #pragma warning disable CS8604
        public void ShouldBeInvalidOnNullItem()
        {
            CreateAuctionItemDTO? item = null;
            var options = new CreateAuctionOptionsDTO(15, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), true, 5, 5, true, 25);
            var auction = new CreateAuctionDTO(item, options);

            var result = auctionValidator.Validate(new CreateAuctionCommand(auction, 1));

            Assert.False(result.IsValid);
        }
        public void ShouldBeInvalidOnNullOptions()
        {
            var item = new CreateAuctionItemDTO("correct", "correct", 1, []);
            CreateAuctionOptionsDTO? options = null;
            var auction = new CreateAuctionDTO(item, options);

            var result = auctionValidator.Validate(new CreateAuctionCommand(auction, 1));

            Assert.That(result.IsValid, Is.EqualTo(false));
        }
    }
}
