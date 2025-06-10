using AuctionHouseAPI.Application.CQRS.Features.Users.Handlers;
using NetArchTest.Rules;

namespace AuctionHouseAPI.Tests.Architecture
{
    [TestFixture]
    public class ApplicationArchitectureTests
    {
        [Test]
        public void ApplicationShouldDependOnDomainLayer()
        {
            var types = Types.InAssembly(typeof(GetUserByIdHandler).Assembly);
            var result = types
                .That()
                .HaveDependencyOn("AuctionHouseAPI.Domain")
                .GetTypes();

            ClassicAssert.IsTrue(result.Any());
        }
        [Test]
        public void ApplicationShouldDependOnSharedLayer()
        {
            var types = Types.InAssembly(typeof(GetUserByIdHandler).Assembly);
            var result = types
                .That()
                .HaveDependencyOn("AuctionHouseAPI.Shared")
                .GetTypes();

            ClassicAssert.IsTrue(result.Any());
        }
        [Test]
        public void ApplicationShouldNotDependOnPresentationLayer()
        {
            var types = Types.InAssembly(typeof(GetUserByIdHandler).Assembly);
            var result = types
                .That()
                .HaveDependencyOn("AuctionHouseAPI.Presentation")
                .GetTypes();

            ClassicAssert.IsFalse(result.Any());
        }
        [Test]
        public void ApplicationShouldNotDependOnMigrationLayer()
        {
            var types = Types.InAssembly(typeof(GetUserByIdHandler).Assembly);
            var result = types
                .That()
                .HaveDependencyOn("AuctionHouseAPI.Migration")
                .GetTypes();

            ClassicAssert.IsFalse(result.Any());
        }
    }
}
