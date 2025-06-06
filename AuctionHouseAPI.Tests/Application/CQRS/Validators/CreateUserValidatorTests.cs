using AuctionHouseAPI.Application.CQRS.Features.Users.Commands;
using AuctionHouseAPI.Application.CQRS.Validators;
using AuctionHouseAPI.Application.DTOs.Create;

namespace AuctionHouseAPI.Tests.Application.CQRS.Validators
{
    public class CreateUserValidatorTests
    {
        [Test]
        public void UsernameShouldBeBetween8and50CharactersLong()
        {
            var shortUsernameDTO = new CreateUserDTO("usrname", "test@email.com", "password123", "fname", "lname");
            var noUsernameDTO = new CreateUserDTO("", "test@email.com", "password123", "fname", "lname");
            var longUsernameDTO = new CreateUserDTO(new string('a', 51), "test@email.com", "password123", "fname", "lname");
            var correctUsernameDTO = new CreateUserDTO("username", "test@email.com", "password123", "fname", "lname");

            var validator = new CreateUserValidator();

            Assert.False(validator.Validate(new CreateUserCommand(shortUsernameDTO)).IsValid);
            Assert.False(validator.Validate(new CreateUserCommand(noUsernameDTO)).IsValid);
            Assert.False(validator.Validate(new CreateUserCommand(longUsernameDTO)).IsValid);
            Assert.True(validator.Validate(new CreateUserCommand(correctUsernameDTO)).IsValid);
        }
        [Test]
        public void PasswordShouldBeBetween10and100CharactersLong()
        {
            var shortPasswordDTO = new CreateUserDTO("username", "test@email.com", "pass", "fname", "lname");
            var noPasswordDTO = new CreateUserDTO("username", "test@email.com", "", "fname", "lname");
            var longPasswordDTO = new CreateUserDTO("username", "test@email.com", new string('a', 101), "fname", "lname");
            var correctPasswordDTO = new CreateUserDTO("username", "test@email.com", "correctpassword", "fname", "lname");

            var validator = new CreateUserValidator();

            Assert.False(validator.Validate(new CreateUserCommand(shortPasswordDTO)).IsValid);
            Assert.False(validator.Validate(new CreateUserCommand(noPasswordDTO)).IsValid);
            Assert.False(validator.Validate(new CreateUserCommand(longPasswordDTO)).IsValid);
            Assert.True(validator.Validate(new CreateUserCommand(correctPasswordDTO)).IsValid);
        }
        [Test]
        public void EmailShouldHaveCorrectStructure()
        {
            var incorrectEmailDTO = new CreateUserDTO("username", "test.email.com", "correctpassword", "fname", "lname");
            var noEmailDTO = new CreateUserDTO("username", "", "correctpassword", "fname", "lname");
            var correctEmailDTO = new CreateUserDTO("username", "test@email.com", "correctpassword", "fname", "lname");

            var validator = new CreateUserValidator();

            Assert.False(validator.Validate(new CreateUserCommand(incorrectEmailDTO)).IsValid);
            Assert.False(validator.Validate(new CreateUserCommand(noEmailDTO)).IsValid);
            Assert.True(validator.Validate(new CreateUserCommand(correctEmailDTO)).IsValid);
        }
        [Test]
        public void FirstNameShouldBeBetween2and100CharactersLong()
        {
            var shortFirstNameDTO = new CreateUserDTO("username", "test@email.com", "correctpassword", "f", "lname");
            var noFirstNameDTO = new CreateUserDTO("username", "test@email.com", "correctpassword", "", "lname");
            var longFirstNamedDTO = new CreateUserDTO("username", "test@email.com", "correctpassword", new string('a', 101), "lname");
            var correctFirstNameDTO = new CreateUserDTO("username", "test@email.com", "correctpassword", "fname", "lname");

            var validator = new CreateUserValidator();

            Assert.False(validator.Validate(new CreateUserCommand(shortFirstNameDTO)).IsValid);
            Assert.False(validator.Validate(new CreateUserCommand(noFirstNameDTO)).IsValid);
            Assert.False(validator.Validate(new CreateUserCommand(longFirstNamedDTO)).IsValid);
            Assert.True(validator.Validate(new CreateUserCommand(correctFirstNameDTO)).IsValid);
        }
        [Test]
        public void LastNameShouldBeBetween2and100CharactersLong()
        {
            var shortLastNameDTO = new CreateUserDTO("username", "test@email.com", "correctpassword", "fname", "l");
            var noLastNameDTO = new CreateUserDTO("username", "test@email.com", "correctpassword", "fname", "");
            var longLastNamedDTO = new CreateUserDTO("username", "test@email.com", "correctpassword", "fname", new string('a', 101));
            var correctLastNameDTO = new CreateUserDTO("username", "test@email.com", "correctpassword", "fname", "lname");

            var validator = new CreateUserValidator();

            Assert.False(validator.Validate(new CreateUserCommand(shortLastNameDTO)).IsValid);
            Assert.False(validator.Validate(new CreateUserCommand(noLastNameDTO)).IsValid);
            Assert.False(validator.Validate(new CreateUserCommand(longLastNamedDTO)).IsValid);
            Assert.True(validator.Validate(new CreateUserCommand(correctLastNameDTO)).IsValid);
        }
    }
}
