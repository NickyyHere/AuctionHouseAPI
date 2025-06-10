using AuctionHouseAPI.Application.CQRS.Features.Auth.Handlers;
using AuctionHouseAPI.Application.CQRS.Features.Auth.Queries;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Shared.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace AuctionHouseAPI.Tests.Application.CQRS.Features.Auth
{
    [TestFixture]
    public class GetAuthTokenQueryTests
    {
        private Mock<IConfiguration> configuration;
        private Mock<IUserRepository> userRepository;
        private Mock<ILogger<GetAuthTokenHandler>> logger;
        private GetAuthTokenHandler handler;
        [SetUp]
        public void Setup()
        {
            configuration = new Mock<IConfiguration>();
            userRepository = new Mock<IUserRepository>();
            logger = new Mock<ILogger<GetAuthTokenHandler>>();
            configuration.Setup(c => c["JwtSettings:Key"]).Returns("key1234567890keykey1234567890key");
            configuration.Setup(c => c["JwtSettings:Issuer"]).Returns("issuer");
            configuration.Setup(c => c["JwtSettings:Audience"]).Returns("audience");
            handler = new GetAuthTokenHandler(configuration.Object, userRepository.Object, logger.Object);
        }
        [Test]
        public async Task ShouldReturnToken()
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("test");
            var user = new User
            {
                Id = 1,
                Username = "test",
                Password = hashedPassword,
                Role = Domain.Enums.UserRole.ROLE_USER
            };
            var query = new GetAuthTokenQuery(new LoginDTO("test", "test"));
            userRepository.Setup(r => r.GetByUsernameAsync("test")).ReturnsAsync(user);

            var result = await handler.Handle(query, default);

            Assert.Multiple(() =>
            {
                ClassicAssert.NotNull(result);
                Assert.That(result, Does.StartWith("Bearer"));
            });
            userRepository.Verify(r => r.GetByUsernameAsync("test"), Times.Once);
        }
        [Test]
        public void ShouldThrowExceptionIfUserDoesNotExist()
        {
            var query = new GetAuthTokenQuery(new LoginDTO("test", "test"));
            userRepository.Setup(r => r.GetByUsernameAsync("test")).ReturnsAsync((User?)null);
            Assert.ThrowsAsync<EntityDoesNotExistException>(() => handler.Handle(query, default));
            userRepository.Verify(r => r.GetByUsernameAsync("test"), Times.Once);
        }
        [Test]
        public void ShouldThrowExceptionIfPasswordHashesDontMatch()
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("test");
            var user = new User
            {
                Id = 1,
                Username = "test",
                Password = hashedPassword,
                Role = Domain.Enums.UserRole.ROLE_USER
            };
            var query = new GetAuthTokenQuery(new LoginDTO("test", "alsoTest"));
            userRepository.Setup(r => r.GetByUsernameAsync("test")).ReturnsAsync(user);

            Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(query, default));
            userRepository.Verify(r => r.GetByUsernameAsync("test"), Times.Once);
        }
    }
}
