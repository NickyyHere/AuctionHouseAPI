using AuctionHouseAPI.Application.CQRS.Features.Bids.Commands;
using AuctionHouseAPI.Application.CQRS.Validators;
using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Domain.Models;
using static MongoDB.Driver.WriteConcern;

namespace AuctionHouseAPI.Tests.Application.CQRS.Validators
{
    [TestFixture]
    public class CreateBidValidatorTests
    {
        private CreateBidValidator validator = new();
        [TestCase(1, true)]
        [TestCase(0, false)]
        public void ShouldValidateAuctionIdCorrectly(int auctionId, bool expected)
        {
            var bid = new CreateBidDTO(auctionId, 5);

            var result = validator.Validate(new CreateBidCommand(bid, 1));

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
        [TestCase(1, true)]
        [TestCase(0, false)]
        public void ShouldValidateAmountCorrectly(decimal amount, bool expected)
        {
            var bid = new CreateBidDTO(1, amount);

            var result = validator.Validate(new CreateBidCommand(bid, 1));

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
        [TestCase(1, true)]
        [TestCase(0, false)]
        public void ShouldValidateUserIdCorrectly(int userId, bool expected)
        {
            var bid = new CreateBidDTO(1, 1);

            var result = validator.Validate(new CreateBidCommand(bid, userId));

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
    }
}
