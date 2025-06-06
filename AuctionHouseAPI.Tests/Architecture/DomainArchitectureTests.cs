using AuctionHouseAPI.Domain.Models;
using NetArchTest.Rules;

namespace AuctionHouseAPI.Tests.Architecture
{
    [TestFixture]
    public class DomainArchitectureTests
    {
        [Test]
        public void DomainShouldNotDependOnSharedLayer()
        {
            var types = Types.InAssembly(typeof(User).Assembly);
            var result = types
                .That()
                .HaveDependencyOn("AuctionHouseAPI.Shared")
                .GetTypes();

            Assert.IsFalse(result.Any());
        }
        [Test]
        public void DomainShouldNotDependOnApplicationLayer()
        {
            var types = Types.InAssembly(typeof(User).Assembly);
            var result = types
                .That()
                .HaveDependencyOn("AuctionHouseAPI.Application")
                .GetTypes();

            Assert.IsFalse(result.Any());
        }
        [Test]
        public void DomainShouldNotDependOnMigrationLayer()
        {
            var types = Types.InAssembly(typeof(User).Assembly);
            var result = types
                .That()
                .HaveDependencyOn("AuctionHouseAPI.Migration")
                .GetTypes();

            Assert.IsFalse(result.Any());
        }
        [Test]
        public void DomainShouldNotDependOnPresentationLayer()
        {
            var types = Types.InAssembly(typeof(User).Assembly);
            var result = types
                .That()
                .HaveDependencyOn("AuctionHouseAPI.Presentation")
                .GetTypes();

            Assert.IsFalse(result.Any());
        }
    }
}
