using AuctionHouseAPI.Application.CQRS.Features.Users.Commands;
using AuctionHouseAPI.Application.CQRS.Features.Users.Handlers;
using AuctionHouseAPI.Application.CQRS.Validators;
using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace AuctionHouseAPI.Tests.Application.CQRS.Features.Users
{
    [TestFixture]
    public class CreateUserCommandTests
    {
        [Test]
        public async Task CreateUserHandlerShouldMapAndCallService()
        {
            var mapper = new Mock<IMapper>();
            var service = new Mock<IUserService>();
            var logger = new Mock<ILogger<CreateUserHandler>>();

            var dto = new CreateUserDTO("test", "test", "test", "test", "test");
            var command = new CreateUserCommand(dto);
            var user = new User { Username = "test" };

            mapper.Setup(m => m.Map<User>(dto)).Returns(user);
            service.Setup(m => m.CreateUserAsync(user)).ReturnsAsync(1);

            var handler = new CreateUserHandler(service.Object, mapper.Object, logger.Object);

            var result = await handler.Handle(command, default);

            Assert.That(result, Is.EqualTo(1));
            mapper.Verify(m => m.Map<User>(dto), Times.Once);
            service.Verify(s => s.CreateUserAsync(user), Times.Once);
        }
    }
}
