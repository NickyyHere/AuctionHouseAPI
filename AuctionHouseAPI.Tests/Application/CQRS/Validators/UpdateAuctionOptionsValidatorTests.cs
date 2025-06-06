using AuctionHouseAPI.Application.CQRS.Features.Auctions.Commands;
using AuctionHouseAPI.Application.CQRS.Validators;
using AuctionHouseAPI.Application.DTOs.Update;

namespace AuctionHouseAPI.Tests.Application.CQRS.Validators
{
    public class UpdateAuctionOptionsValidatorTests
    {
        private UpdateAuctionOptionsValidator validator = new();
        [TestCase(5, true)]
        [TestCase(0, false)]
        public void ShouldValidateStartingPriceCorrectly(decimal price, bool expected)
        {
            var options = new UpdateAuctionOptionsDTO(price, null, null, null, null, null, null, null);

            var result = validator.Validate(new UpdateAuctionOptionsCommand(options, 1, 1));

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
        private static readonly object[] Dates =
        {
            new object[] { DateTime.UtcNow.AddDays(2), true },
            new object[] { DateTime.UtcNow.AddDays(-1), false }
        };
        [TestCaseSource(nameof(Dates))]
        public void ShouldValidateStartDateCorrectly(DateTime startDate, bool expected)
        {
            var options = new UpdateAuctionOptionsDTO(null, startDate, null, null, null, null, null, null);

            var result = validator.Validate(new UpdateAuctionOptionsCommand(options, 1, 1));

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
        [TestCaseSource(nameof(Dates))]
        public void ShouldValidateEndDateCorrectly(DateTime endDate, bool expected)
        {
            var options = new UpdateAuctionOptionsDTO(null, null, endDate, null, null, null, null, null);

            var result = validator.Validate(new UpdateAuctionOptionsCommand(options, 1, 1));

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
        [TestCase(5, true)]
        [TestCase(4, false)]
        public void SHouldValidateMinimumOutbidCorrectly(int minimumOutbid, bool expected)
        {
            var options = new UpdateAuctionOptionsDTO(null, null, null, null, null, minimumOutbid, null, null);

            var result = validator.Validate(new UpdateAuctionOptionsCommand(options, 1, 1));

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
        [TestCase(10, false)]
        [TestCase(20, true)]
        public void ShouldValidateBINPriceCorrectly(decimal price, bool expected)
        {
            var options = new UpdateAuctionOptionsDTO(15, null, null, null, null, null, null, price);

            var result = validator.Validate(new UpdateAuctionOptionsCommand(options, 1, 1));

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
    }
}
