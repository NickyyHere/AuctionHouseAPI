using AuctionHouseAPI.Application.CQRS.Features.Categories.Commands;
using AuctionHouseAPI.Application.CQRS.Validators;
using AuctionHouseAPI.Application.DTOs.Update;

namespace AuctionHouseAPI.Tests.Application.CQRS.Validators
{
    [TestFixture]
    public class UpdateCategoryValidatorTests
    {
        [TestCase("correct", true)]
        [TestCase("f", false)]
        public void ShouldValidateNameCorrectly(string name, bool expected)
        {
            var validator = new UpdateCategoryValidator();

            var update = new UpdateCategoryDTO(name, null);

            var result = validator.Validate(new EditCategoryCommand(update, 1));

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
    }
}
