using AuctionHouseAPI.Application.CQRS.Features.Users.Commands;
using AuctionHouseAPI.Application.CQRS.Validators;
using AuctionHouseAPI.Application.DTOs.Update;

namespace AuctionHouseAPI.Tests.Application.CQRS.Validators
{
    [TestFixture]
    public class UpdateUserValidatorTests
    {
        private UpdateUserValidator validator = new();
        [TestCase("Correct@email.com", true)]
        [TestCase("Incorrect.email.com", false)]
        public void ShouldValidateEmailCorrectly(string email, bool expected)
        {
            var update = new UpdateUserDTO(email, null, null, null);

            var result = validator.Validate(new UpdateUserCommand(update, 1));

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
        [TestCase("correctpassword", true)]
        [TestCase("password", false)]
        public void ShouldValidatePasswordCorrectly(string password, bool expected)
        {
            var update = new UpdateUserDTO(null, password, null, null);

            var result = validator.Validate(new UpdateUserCommand(update, 1));

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
        [TestCase("correct", true)]
        [TestCase("f", false)]
        public void ShouldValidateFirstNameCorrectly(string fName, bool expected)
        {
            var update = new UpdateUserDTO(null, null, fName, null);

            var result = validator.Validate(new UpdateUserCommand(update, 1));

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
        [TestCase("correct", true)]
        [TestCase("f", false)]
        public void ShouldValidateLastNameCorrectly(string lName, bool expected)
        {
            var update = new UpdateUserDTO(null, null, null, lName);

            var result = validator.Validate(new UpdateUserCommand(update, 1));

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
    }
}
