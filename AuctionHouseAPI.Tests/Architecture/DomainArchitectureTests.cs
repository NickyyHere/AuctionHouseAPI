using AuctionHouseAPI.Domain.Models;
using NetArchTest.Rules;

namespace AuctionHouseAPI.Tests.Architecture
{
    [TestFixture]
    public class DomainArchitectureTests
    {
        [Test]
        public void DomainShouldDependOnSharedLayer()
        {
            var types = Types.InAssembly(typeof(User).Assembly);
            var result = types
                .That()
                .HaveDependencyOn("AuctionHouseAPI.Shared")
                .GetTypes();

            ClassicAssert.IsTrue(result.Any());
        }
        [Test]
        public void DomainShouldNotDependOnApplicationLayer()
        {
            var types = Types.InAssembly(typeof(User).Assembly);
            var result = types
                .That()
                .HaveDependencyOn("AuctionHouseAPI.Application")
                .GetTypes();

            ClassicAssert.IsFalse(result.Any());
        }
        [Test]
        public void DomainShouldNotDependOnMigrationLayer()
        {
            var types = Types.InAssembly(typeof(User).Assembly);
            var result = types
                .That()
                .HaveDependencyOn("AuctionHouseAPI.Migration")
                .GetTypes();

            ClassicAssert.IsFalse(result.Any());
        }
        [Test]
        public void DomainShouldNotDependOnPresentationLayer()
        {
            var types = Types.InAssembly(typeof(User).Assembly);
            var result = types
                .That()
                .HaveDependencyOn("AuctionHouseAPI.Presentation")
                .GetTypes();

            ClassicAssert.IsFalse(result.Any());
        }
    }
}
