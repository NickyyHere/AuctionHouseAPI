using AuctionHouseAPI.Application.CQRS.Features.Categories.Commands;
using AuctionHouseAPI.Application.CQRS.Validators;
using AuctionHouseAPI.Application.DTOs.Create;

namespace AuctionHouseAPI.Tests.Application.CQRS.Validators
{
    [TestFixture]
    public class CreateCategoryValidatorTests
    {
        private CreateCategoryValidator validator = new();
        [Test]
        [TestCase("a", false)]
        [TestCase("abc", true)]
        [TestCase("abcdeabcdeabcdeabcdea", false)]
        public void ShouldValidateNameCorrectly(string name, bool expected)
        {
            var category = new CreateCategoryDTO(name, "desc");

            var result = validator.Validate(new CreateCategoryCommand(category));

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
    }
}
